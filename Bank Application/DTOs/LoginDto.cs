using System.ComponentModel.DataAnnotations;
namespace Bank_Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "الإيميل إجباري")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال إيميل صحيح")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "كلمة المرور إجبارية")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = null!;
    }
}
