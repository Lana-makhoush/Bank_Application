using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class SubAccountResponseDto
    {
        public int SubAccountId { get; set; }
        public int ParentAccountId { get; set; }
        public decimal? DailyWithdrawalLimit { get; set; }
        public decimal? TransferLimit { get; set; }
        public string? UsageAreas { get; set; }
        public string? UserPermissions { get; set; }
        [Required(ErrorMessage = "الرصيد مطلوب")]

        [Range(0, double.MaxValue, ErrorMessage = "الرصيد يجب أن يكون رقمًا موجبًا")]
        public decimal? Balance { get; set; }
        [Required(ErrorMessage = "تاريخ الإنشاء مطلوب")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? StatusName { get; set; }
    }

}
