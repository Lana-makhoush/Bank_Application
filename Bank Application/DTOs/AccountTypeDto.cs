using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class AccountTypeDto
    {
        [MaxLength(30)]
        [Required(ErrorMessage = "اسم نوع الحساب مطلوب")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z]+$",
            ErrorMessage = "اسم الحساب يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string TypeName { get; set; }

        [Required(ErrorMessage = "الفائدة السنوية مطلوبة")]
        [Range(50, double.MaxValue, ErrorMessage = "الفائدة السنوية يجب أن تكون رقمًا موجبًا")]
        public decimal AnnualInterestRate { get; set; }

        [Required(ErrorMessage = "حد السحب اليومي مطلوب")]
        [Range(200, double.MaxValue, ErrorMessage = "حد السحب اليومي يجب أن يكون رقمًا موجبًا")]
        public decimal DailyWithdrawalLimit { get; set; }

        [Required(ErrorMessage = "الرسوم الشهرية مطلوبة")]
        [Range(100, double.MaxValue, ErrorMessage = "الرسوم الشهرية يجب أن تكون رقمًا موجبًا")]
        public decimal MonthlyFee { get; set; }

        [Required(ErrorMessage = "الشروط والأحكام مطلوبة")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "الشروط والأحكام يجب أن تكون بين 10 و 500 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s.,\-]*$",
            ErrorMessage = "الشروط والأحكام يمكن أن تحتوي على أحرف عربية، إنجليزية، أرقام ومسافات وعلامات ترقيم . , - فقط")]
        public string TermsAndConditions { get; set; }
    }
}
