using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bank_Application.Models
{
    public class Employee
    {
        [Key]
        public int? EmployeeId { get; set; }
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الاسم الأول يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]*$",
            ErrorMessage = "الاسم الأوسط يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الاسم الأخير يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [RegularExpression(@"^[0-9]{12}$",
            ErrorMessage = "رقم الهوية يجب أن يكون 12 رقماً")]
        public string? IdentityNumber { get; set; }

        public string? IdentityImage { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^09[0-9]{8}$",
            ErrorMessage = "رقم الهاتف يجب أن يبدأ بـ 09 ويتكون من 10 أرقام")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string? Password { get; set; }

        [RegularExpression(@"^(Manager|Admin|Teller)?$",
        ErrorMessage = "الدور يجب أن يكون: Manager أو Admin أو Teller")]
        public string? Role { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Report>? Reports { get; set; }
        public string? Email { get; set; }


    }
}
