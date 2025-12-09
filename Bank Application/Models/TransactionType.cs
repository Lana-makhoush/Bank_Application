using System.ComponentModel.DataAnnotations;

namespace Bank_Application.Models
{
    public class TransactionType
    {
        [Key]
        public int? TransactionTypeId { get; set; }

        [StringLength(40, MinimumLength = 2, ErrorMessage = "اسم نوع المعاملة يجب أن يكون بين 2 و 40 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z]+$",
            ErrorMessage = "اسم نوع المعاملة يجب أن يحتوي فقط على أحرف عربية أو إنجليزية")]
        public string? TypeName { get; set; } = string.Empty;

        public ICollection<TransactionLog>? TransactionLogs { get; set; }
    }
}

