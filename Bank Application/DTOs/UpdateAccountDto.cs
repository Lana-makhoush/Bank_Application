using System.ComponentModel.DataAnnotations;

namespace Bank_Application.Dtos
{
    public class UpdateAccountDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "الرصيد يجب أن يكون رقمًا موجبًا")]
        public decimal? Balance { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
