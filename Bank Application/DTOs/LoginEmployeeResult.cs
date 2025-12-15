    namespace Bank_Application.DTOs
    {
        public class LoginEmployeeResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;

            public string? Token { get; set; }
            public DateTime? Expiry { get; set; }
            public bool MustChangePassword { get; set; }
            public string? Role { get; set; }

            public static LoginEmployeeResult Fail(string message)
            {
                return new LoginEmployeeResult
                {
                    Success = false,
                    Message = message
                };
            }

            public static LoginEmployeeResult Ok(
                string token,
                DateTime expiry,
                bool mustChangePassword,
                string role)
            {
                return new LoginEmployeeResult
                {
                    Success = true,
                    Token = token,
                    Expiry = expiry,
                    MustChangePassword = mustChangePassword,
                    Role = role
                };
            }
        }
    }

