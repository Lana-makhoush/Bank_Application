using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Bank_Application.Models
{
    public class SubAccount
    {
        [Key]
        public int? SubAccountId { get; set; }

        public int? ParentAccountId { get; set; }
        public Account? ParentAccount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "حد السحب اليومي يجب أن يكون رقمًا موجبًا")]
        public decimal? DailyWithdrawalLimit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "حد التحويل يجب أن يكون رقمًا موجبًا")]
        public decimal? TransferLimit { get; set; }

        [StringLength(200, MinimumLength = 2, ErrorMessage = "مناطق الاستخدام يجب أن تكون بين 2 و 200 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z\s,.-]*$",
            ErrorMessage = "مناطق الاستخدام يجب أن تحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? UsageAreas { get; set; } = string.Empty;

        [StringLength(200, MinimumLength = 2, ErrorMessage = "صلاحيات المستخدم يجب أن تكون بين 2 و 200 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z\s,.-]*$",
            ErrorMessage = "صلاحيات المستخدم يجب أن تحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? UserPermissions { get; set; } = string.Empty;
    }
}
