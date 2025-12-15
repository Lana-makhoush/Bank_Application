using Bank_Application.design_pattern.Factory;
using Bank_Application.DTOs;
using Bank_Application.Repositories;
using Bank_Application.services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Bank_Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;
        public EmployeeService(
            IEmployeeRepository repo,
            IEmailService emailService,
            IConfiguration config,IJwtService jwtService)
        {
            _repo = repo;
            _emailService = emailService;
            _config = config;
            _jwtService = jwtService;
        }

        public async Task<ServiceResult> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            
            if (await _repo.EmailExistsAsync(dto.Email))
                return ServiceResult.Fail("الإيميل مستخدم مسبقاً");

            
            if (await _repo.UsernameExistsAsync(dto.Username))
                return ServiceResult.Fail("اسم المستخدم مستخدم مسبقاً");

           
            var tempPassword = Guid.NewGuid().ToString("N")[..8];
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(tempPassword);

            
            var employee = EmployeeFactory.Create(
                dto.Role,
                dto,
                hashedPassword);

            await _repo.AddAsync(employee);

            
            await _emailService.SendEmailAsync(
                dto.Email,
                "بيانات الدخول",
                $"Username: {dto.Username}\nPassword: {tempPassword}"
            );

            return ServiceResult.Ok("تم إنشاء حساب الموظف بنجاح وإرسال بيانات الدخول إلى الايميل");
        }




        public async Task<LoginEmployeeResult> LoginEmployeeAsync(LoginEmployeeDto dto)
        {
            var employee = await _repo.GetByUsernameAsync(dto.Username);

            if (employee == null)
                return LoginEmployeeResult.Fail("اسم المستخدم أو كلمة المرور غير صحيحة");

            if (!employee.IsActive)
                return LoginEmployeeResult.Fail("الحساب غير مفعل");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, employee.Password))
                return LoginEmployeeResult.Fail("اسم المستخدم أو كلمة المرور غير صحيحة");

            var token = _jwtService.GenerateToken(employee.EmployeeId,employee.Role, out DateTime exp);

            return LoginEmployeeResult.Ok(
                token,
                exp,
                employee.MustChangePassword,
                employee.Role!
            );
        }


        public async Task<ServiceResult> ChangePasswordAsync(
    int employeeId,
    ChangePasswordDto dto)
        {
            var employee = await _repo.GetByIdAsync(employeeId);

            if (employee == null)
                return ServiceResult.Fail("الموظف غير موجود");

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, employee.Password))
                return ServiceResult.Fail("كلمة المرور القديمة غير صحيحة");

            employee.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            employee.MustChangePassword = false;

            await _repo.UpdateAsync(employee);

            return ServiceResult.Ok("تم تغيير كلمة المرور بنجاح");
        }



    
        }
    }


