

    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace Bank_Application.services
    {
        public class JwtService : IJwtService
        {
            private readonly IConfiguration _config;

            public JwtService(IConfiguration config)
            {
                _config = config;
            }

  
        public string GenerateToken(int id, string role, out DateTime exp)
        {
            var jwt = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Secret"]!));

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            exp = DateTime.UtcNow.AddMinutes(
                int.Parse(jwt["ExpiryMinutes"]!));

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

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
    }


