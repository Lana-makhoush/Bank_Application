

using Bank_Application.DTOs;
using Bank_Application.Models;

namespace Bank_Application.design_pattern.Factory
{
    public static class EmployeeFactory
    {
        public static Employee Create(
            string role,
            CreateEmployeeDto dto,
            string hashedPassword)
        {
            if (role != "Manager" && role != "Teller")
                throw new ArgumentException("الدور المرسل غير مسموح");

            return new Employee
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = role,

                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                IdentityNumber = dto.IdentityNumber,
                Phone = dto.Phone,

                Password = hashedPassword,
                IsActive = true,
                MustChangePassword = true,
                CreatedAt = DateTime.Now
            };
        }
    }
}
