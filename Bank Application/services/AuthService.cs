using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bank_Application.services
{
   

    public class AuthService : IAuthService
    {
        private readonly IClientAuthRepository _clientRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IConfiguration _config;
        private readonly IEmailService _email;

        public AuthService(
            IClientAuthRepository clientRepo,
            IEmployeeRepository employeeRepo,
            IConfiguration config,
            IEmailService email)
        {
            _clientRepo = clientRepo;
            _employeeRepo = employeeRepo;
            _config = config;
            _email = email;
        }

        public async Task<(bool Success, string? Message)> RegisterClientAsync(ClientRegisterDto dto)
        {
            if (await _clientRepo.GetByEmailAsync(dto.Email) != null)
                return (false, "الايميل مستخدم");

            if (await _clientRepo.GetByUsernameAsync(dto.Username) != null)
                return (false, "اسم المستخدم محجوز");

            var client = new Client
            {
                Username = dto.Username,
                Email = dto.Email,  
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();
            client.OtpCode = otp;
            client.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

            await _clientRepo.AddAsync(client);

            // SEND OTP EMAIL
            await _email.SendEmailAsync(dto.Email,
                "رمز التحقق OTP",
                $"رمز التحقق هو: {otp}\nصالح لمدة 10 دقائق.");

            return (true, "تم إرسال رمز التحقق إلى بريدك.");
        }

        public async Task<(bool Success, string? Message)> VerifyOtpAsync(VerifyOtpDto dto)
        {
            var user = await _clientRepo.GetByEmailAsync(dto.Email);
            if (user == null) return (false, "المستخدم غير موجود");

            if (user.OtpCode != dto.OtpCode)
                return (false, "OTP خطأ");

            if (user.OtpExpiry < DateTime.UtcNow)
                return (false, "انتهت صلاحية الرمز");

            user.IsVerified = true;
            user.OtpCode = null;
            user.OtpExpiry = null;

            await _clientRepo.UpdateAsync(user);

            return (true, "تم تفعيل الحساب");
        }

        public async Task<(bool Success, AuthResponseDto? Auth, string? Message)> LoginAsync(LoginDto dto)
        {
           
            var client = await _clientRepo.GetByEmailAsync(dto.Email);
            if (client == null)
                return (false, null, "الايميل غير موجود");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, client.Password))
                return (false, null, "كلمة المرور خاطئة");

            var token = GenerateJwt(client.ClientId, "User", out DateTime expiry);

            return (true, new AuthResponseDto
            {
                Token = token,
                Expiry = expiry,
                Role = "User",
                UserId = client.ClientId,
                Username = client.Username,
                Email = client.Email
            }, null);
        }


        private string GenerateJwt(int id, string role, out DateTime exp)
        {
            var jwt = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            exp = DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiryMinutes"]!));

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Role, role)
        };

            var token = new JwtSecurityToken(
                jwt["Issuer"],
                jwt["Audience"],
                claims,
                expires: exp,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
