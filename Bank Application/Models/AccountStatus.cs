using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Application.Models
{
    public class AccountStatus
    {
        [Key]
        public int? AccountStatusId { get; set; }

        [Required(ErrorMessage = "اسم الحالة مطلوب")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "اسم الحالة يجب أن يكون بين 2 و 30 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z]+$",
            ErrorMessage = "اسم الحالة يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? StatusName { get; set; } = string.Empty;
        public ICollection<Account>? Accounts { get; set; }
        public ICollection<SubAccount>? SubAccounts { get; set; }

    }
}
