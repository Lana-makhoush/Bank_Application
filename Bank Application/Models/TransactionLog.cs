using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Application.Models
{
    public class TransactionLog
    {
        [Key]
        public int TransactionLogId { get; set; }

        //[Required(ErrorMessage = "نوع المعاملة مطلوب")]
        public int? TransactionTypeId { get; set; }

        [ForeignKey("TransactionTypeId")]
        public TransactionType? TransactionType { get; set; }

        public int? SenderAccountId { get; set; }

        public int? ReceiverAccountId { get; set; }

       

        //[Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(0.01, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        public decimal? Amount { get; set; }

        public DateTime ?TransactionDate { get; set; } = DateTime.Now;

        [StringLength(200, ErrorMessage = "الوصف  الخاص بعملية التحويل بين الحسابات يجب ألا يتجاوز 200 حرف")]
        public string? Description { get; set; }
        public int? ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }
    }
}
