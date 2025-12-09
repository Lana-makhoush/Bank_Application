using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Application.Models
{
    public class SupportTicket
    {
        [Key]
        public int? TicketId { get; set; }

        public int? ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "الموضوع يجب أن يكون بين 3 و 100 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s]+$",
            ErrorMessage = "الموضوع يجب أن يحتوي على أحرف عربية أو إنجليزية أو أرقام فقط")]
        public string? Subject { get; set; }

        [StringLength(1000, ErrorMessage = "الوصف طويل جدًا")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s.,!?-]+$",
            ErrorMessage = "الوصف يجب أن يكون نصًا عربيًا أو إنجليزيًا فقط")]
        public string? Description { get; set; }

        [RegularExpression(@"^(Open|InProgress|Closed)$",
            ErrorMessage = "الحالة يجب أن تكون: Open, InProgress, Closed")]
        public string? Status { get; set; } = "Open";

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
