using System.Collections.Generic;

namespace Bank_Application.DTOs
{
    public class AccountHierarchyDto
    {
        public int? AccountId { get; set; }
        public string? AccountName { get; set; } = "حساب رئيسي";
        public List<SubAccountResponseDto> SubAccounts { get; set; } = new();
        public decimal Balance { get; set; }
    }
}
