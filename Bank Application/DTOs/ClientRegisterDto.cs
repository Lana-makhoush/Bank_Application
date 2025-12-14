using System.ComponentModel.DataAnnotations;
namespace Bank_Application.DTOs
{
 

    public class ClientRegisterDto
    {
        [Required(ErrorMessage = "اسم المستخدم إجباري")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$",
            ErrorMessage = "اسم المستخدم يجب أن يحتوي على أحرف إنكليزية وأرقام و _ فقط")]
        [MinLength(3, ErrorMessage = "اسم المستخدم يجب أن يكون 3 أحرف على الأقل")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "الإيميل إجباري")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال إيميل صحيح")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "كلمة المرور إجبارية")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
            ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف واحد على الأقل ورقم واحد على الأقل")]
        public string Password { get; set; } = null!;
    }

}
