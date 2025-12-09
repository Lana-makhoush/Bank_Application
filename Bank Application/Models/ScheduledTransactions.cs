using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Application.Models
{
    public class ScheduledTransaction
    {
        [Key]
        public int? ScheduledTransactionId { get; set; }

        public int? AccountId { get; set; }  

        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        public decimal? Amount { get; set; }

        [RegularExpression(@"^(Daily|Weekly|Monthly|Yearly)$",
            ErrorMessage = "نوع التكرار يجب أن يكون Daily, Weekly, Monthly, أو Yearly")]
        public string RecurrenceType { get; set; }

        [Required(ErrorMessage = "تاريخ التنفيذ التالي مطلوب")]
        public DateTime? NextExecutionDate { get; set; }

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
