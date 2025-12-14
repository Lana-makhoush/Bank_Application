using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Bank_Application.Models
{
    public class Client
    {
        public int ClientId { get; set; }

       
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الاسم الأول يجب أن يحتوي على أحرف عربية أو إنكليزية فقط")]
        public string? FirstName { get; set; }
        

        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]*$",
            ErrorMessage = "الاسم الأوسط يجب أن يحتوي على أحرف عربية أو إنكليزية فقط")]
        public string? MiddleName { get; set; }

       
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الاسم الأخير يجب أن يحتوي على أحرف عربية أو إنكليزية فقط")]
        public string? LastName { get; set; }

       
        [RegularExpression(@"^[0-9]{12}$",
            ErrorMessage = "رقم الهوية يجب أن يكون 12 رقماً")]
        public string? IdentityNumber { get; set; }
        [NotMapped]
       

        public IFormFile? IdentityImage { get; set; }
        public string? IdentityImagePath { get; set; }
       

        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "مصدر الدخل يجب أن يحتوي على أحرف فقط")]
        public string? IncomeSource { get; set; }
        

        [Range(200, (double)decimal.MaxValue, ErrorMessage = "الدخل الشهري يجب أن يكون رقماً فقط")]
        public decimal? MonthlyIncome { get; set; }
        

        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$",
            ErrorMessage = "الغرض من الحساب يجب أن يحتوي على أحرف فقط")]
        public string? AccountPurpose { get; set; }

       
        [RegularExpression(@"^09[0-9]{8}$",
            ErrorMessage = "رقم الهاتف يجب أن يبدأ بـ 09 ويتكون من 10 أرقام")]
        public string? Phone { get; set; }

       

        [RegularExpression(@"^[a-zA-Z0-9_]+$",
            ErrorMessage = "اسم المستخدم يجب أن يحتوي على أحرف إنكليزية وأرقام و _ فقط")]
        public string? Username { get; set; }

       

        [MinLength(6, ErrorMessage = "الحد الأدنى لطول كلمة المرور هو 6 أحرف")]
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public bool IsVerified { get; set; } = false;
        public ICollection<SupportTicket>? SupportTickets { get; set; }
        public ICollection<TransactionLog>? TransactionLogs { get; set; }


    }
}
