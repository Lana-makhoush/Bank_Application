using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Application.Models
{
    public class Report
    {
        [Key]
        public int? ReportId { get; set; }

        [RegularExpression(@"^(DailyTransactions|AccountSummary|AuditLog)$",
            ErrorMessage = "نوع التقرير يجب أن يكون DailyTransactions أو AccountSummary أو AuditLog")]
        public string? ReportType { get; set; }

        public int? EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(500)]
        public string? FilePath { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [RegularExpression(@"^(Generated|Failed|Pending)$",
            ErrorMessage = "الحالة يجب أن تكون Generated أو Failed أو Pending")]
        public string? Status { get; set; } = "Pending";
    }
}
