using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class SubAccountCreateDto
    {

        public int ParentAccountId { get; set; }
        [Required(ErrorMessage = "حد السحب اليومي اجباري")]
        [Range(0, double.MaxValue, ErrorMessage = "حد السحب اليومي يجب أن يكون رقمًا موجبًا")]
        public decimal? DailyWithdrawalLimit { get; set; }
        [Required(ErrorMessage = "حد  التحويل اجباري")]

        [Range(0, double.MaxValue, ErrorMessage = "حد التحويل يجب أن يكون رقمًا موجبًا")]
        public decimal? TransferLimit { get; set; }
        [Required(ErrorMessage = "اماكن الاستخدام اجبارية")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z\s]+$", ErrorMessage = "اماكن الاستخدام يجب أن تحتوي محارف فقط")]
        public string? UsageAreas { get; set; }

        [Required(ErrorMessage = "شروط الاستخدام اجبارية")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z\s]+$", ErrorMessage = "شروط الاستخدام يجب أن تحتوي محارف فقط")]
        public string? UserPermissions { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "الرصيد يجب أن يكون رقمًا موجبًا")]
        [Required(ErrorMessage = "حقل الرصيد مطلوب")]

        public decimal? Balance { get; set; }
        [Required(ErrorMessage = "تاريخ الإنشاء مطلوب")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

    }
}