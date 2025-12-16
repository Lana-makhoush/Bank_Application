using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Application.Models
{
    public class SupportTicketReply
    {
        [Key]
        public int? ReplyId { get; set; }

        public int? TicketId { get; set; }

        [ForeignKey(nameof(TicketId))]
        public SupportTicket? SupportTicket { get; set; }

        [Required(ErrorMessage = "نص الرد مطلوب")]
        [StringLength(1000, ErrorMessage = "نص الرد طويل جدًا")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s.,!?-]+$",
            ErrorMessage = "الرد يجب أن يكون نصًا عربيًا أو إنجليزيًا فقط")]
        public string? ReplyText { get; set; }

        public int? EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        public DateTime? RepliedAt { get; set; } = DateTime.Now;
    }
}
