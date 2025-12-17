using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Application.Models
{
    public class ClientAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  

        public int? ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }

        public int? AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "الرصيد يجب أن يكون رقمًا موجبًا")]
        public decimal? Balance { get; set; }

        [Required(ErrorMessage = "تاريخ الإنشاء مطلوب")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
