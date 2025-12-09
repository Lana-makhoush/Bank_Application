using System.ComponentModel.DataAnnotations;

namespace Bank_Application.Models
{
    public class ClientAccount
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        public int? AccountId { get; set; }
        public Account? Account { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "الرصيد يجب أن يكون رقمًا موجبًا")]
        public decimal? Balance { get; set; }
        [Required(ErrorMessage = "تاريخ الإنشاء مطلوب")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
