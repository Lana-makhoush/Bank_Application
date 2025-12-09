using System.ComponentModel.DataAnnotations;

namespace Bank_Application.Models
{
    public class AccountType
    {
        [Key]
        public int? AccountTypeId { get; set; }

        [Required(ErrorMessage = "اسم نوع الحساب مطلوب")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "اسم الحساب يجب أن يكون بين 2 و 30 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z]+$",
            ErrorMessage = "اسم الحساب يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? TypeName { get; set; } = string.Empty;
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "يجب إدخال رقم فقط في هذا الحقل")]

        [Range(50, 100, ErrorMessage = "الفائدة السنوية يجب أن تكون بين 0 و 100")]
        public decimal? AnnualInterestRate { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "يجب إدخال رقم فقط في هذا الحقل")]

        [Range(50, double.MaxValue, ErrorMessage = "حد السحب اليومي يجب أن يكون رقمًا موجبًا")]
        public decimal? DailyWithdrawalLimit { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "يجب إدخال رقم فقط في هذا الحقل")]

        [Range(50, double.MaxValue, ErrorMessage = "الرسوم الشهرية يجب أن تكون رقمًا موجبًا")]
        public decimal? MonthlyFee { get; set; }

        [Required(ErrorMessage = "الشروط والأحكام مطلوبة")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "الشروط والأحكام يجب أن تكون بين 10 و 500 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s.,\-]*$",
            ErrorMessage = "الشروط والأحكام يمكن أن تحتوي على أحرف عربية، إنجليزية، أرقام، ومسافات، وعلامات ترقيم . , -")]
        public string? TermsAndConditions { get; set; } = string.Empty;
        public ICollection<Account>? Accounts { get; set; }
        public ICollection<AccountTypeFeature>? AccountTypeFeatures { get; set; }

    }
}
