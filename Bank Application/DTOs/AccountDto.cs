using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class AccountDto
    {
        [Required(ErrorMessage = "الرصيد مطلوب")]

        [Range(0, double.MaxValue, ErrorMessage = "الرصيد يجب أن يكون رقمًا موجبًا")]
        public decimal? Balance { get; set; }
        [Required(ErrorMessage = "تاريخ الإنشاء مطلوب")]
        public DateTime? CreatedAt { get; set; }
    }
}

