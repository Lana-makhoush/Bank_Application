using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Application.DTOs
{
    public class ClientDto
    {
//
        [Required(ErrorMessage = "حقل الاسم الأول إجباري")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
             ErrorMessage = "الاسم الأول يجب أن يحتوي على أحرف عربية أو إنكليزية فقط")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "حقل الاسم الاوسط إجباري")]

        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]*$",
            ErrorMessage = "الاسم الأوسط يجب أن يحتوي على أحرف عربية أو إنكليزية فقط")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "حقل الاسم الأخير إجباري")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الاسم الأخير يجب أن يحتوي على أحرف عربية أو إنكليزية فقط")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "رقم الهوية إجباري")]
        [RegularExpression(@"^[0-9]{12}$",
            ErrorMessage = "رقم الهوية يجب أن يكون 12 رقماً")]
        public string? IdentityNumber { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "صورة الهوية اجبارية")]

        public IFormFile? IdentityImage { get; set; }
        public string? IdentityImagePath { get; set; }
        [Required(ErrorMessage = "مصدر الدخل اجباري")]

        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
           ErrorMessage = "مصدر الدخل يجب أن يحتوي على أحرف فقط")]
        public string? IncomeSource { get; set; }
        [Required(ErrorMessage = "الدخل الشهري اجباري")]

        [Range(200, (double)decimal.MaxValue, ErrorMessage = "الدخل الشهري يجب أن يكون رقماً فقط")]
        public string? MonthlyIncome { get; set; }
        [Required(ErrorMessage = "الغرض من الحساب اجباري")]

        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الغرض من الحساب يجب أن يحتوي على أحرف فقط")]
        public string? AccountPurpose { get; set; }

        [Required(ErrorMessage = "رقم الهاتف إجباري")]
        [RegularExpression(@"^09[0-9]{8}$",
            ErrorMessage = "رقم الهاتف يجب أن يبدأ بـ 09 ويتكون من 10 أرقام")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "اسم المستخدم اجباري")]

        [RegularExpression(@"^[a-zA-Z0-9_]+$",
            ErrorMessage = "اسم المستخدم يجب أن يحتوي على أحرف إنكليزية وأرقام و _ فقط")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "كلمة المرور اجبارية")]

        [MinLength(6, ErrorMessage = "الحد الأدنى لطول كلمة المرور هو 6 أحرف")]
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
