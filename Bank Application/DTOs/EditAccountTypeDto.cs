using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class EditAccountTypeDto
    {
        [MaxLength(30)]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z]+$",
            ErrorMessage = "اسم الحساب يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? TypeName { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "يجب إدخال رقم فقط في هذا الحقل")]
        [Range(50, double.MaxValue, ErrorMessage = "الفائدة السنوية يجب أن تكون رقمًا موجبًا")]
        public string? AnnualInterestRate { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "يجب إدخال رقم فقط في هذا الحقل")]

        [Range(200, double.MaxValue, ErrorMessage = "حد السحب اليومي يجب أن يكون رقمًا موجبًا")]
        public string? DailyWithdrawalLimit { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "يجب إدخال رقم فقط في هذا الحقل")]

        [Range(100, double.MaxValue, ErrorMessage = "الرسوم الشهرية يجب أن تكون رقمًا موجبًا")]
        public string? MonthlyFee { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "الشروط والأحكام يجب أن تكون بين 10 و 500 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s.,\-]*$",
            ErrorMessage = "الشروط والأحكام يمكن أن تحتوي على أحرف عربية، إنجليزية، أرقام ومسافات وعلامات ترقيم . , - فقط")]
        public string? TermsAndConditions { get; set; }
    }
}
