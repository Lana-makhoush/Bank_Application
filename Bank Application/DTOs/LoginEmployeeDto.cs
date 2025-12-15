using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class LoginEmployeeDto
    {          
    
            [Required]
            public string Username { get; set; } = null!;

            [Required]
            public string Password { get; set; } = null!;
        }
    
}
