
using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$",
            ErrorMessage = "اسم المستخدم يجب أن يحتوي على أحرف إنجليزية، أرقام أو _ فقط")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "الإيميل مطلوب")]
        [EmailAddress(ErrorMessage = "الإيميل غير صالح")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "الدور مطلوب")]
        [RegularExpression(@"^(Manager|Teller)$",
            ErrorMessage = "الدور يجب أن يكون Manager أو Teller فقط")]
        public string Role { get; set; } = null!; // Manager أو Teller

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الاسم الأول يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string FirstName { get; set; } = null!;

        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]*$",
            ErrorMessage = "الاسم الأوسط يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الاسم الأخير يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [RegularExpression(@"^[0-9]{12}$",
            ErrorMessage = "رقم الهوية يجب أن يكون 12 رقماً")]
        public string IdentityNumber { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^09[0-9]{8}$",
            ErrorMessage = "رقم الهاتف يجب أن يبدأ بـ 09 ويتكون من 10 أرقام")]
        public string Phone { get; set; } = null!;
    }
}
