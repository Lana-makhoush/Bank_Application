using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class SubAccountUpdateDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "حد السحب اليومي يجب أن يكون رقمًا موجبًا")]
        public string? DailyWithdrawalLimit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "حد التحويل يجب أن يكون رقمًا موجبًا")]
        public string? TransferLimit { get; set; }
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z\s]+$", ErrorMessage = "اماكن الاستخدام يجب أن تحتوي محارف فقط")]
        public string? UsageAreas { get; set; }

        [RegularExpression(@"^[\u0621-\u064A a-zA-Z\s]+$", ErrorMessage = "شروط الاستخدام يجب أن تحتوي محارف فقط")]
        public string? UserPermissions { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "الرصيد يجب أن يكون رقمًا موجبًا")]
        public string? Balance { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}

