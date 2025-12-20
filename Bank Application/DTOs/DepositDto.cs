namespace Bank_Application.DTOs
{
    public class DepositDto
    {
        public int AccountNumber { get; set; } // رقم الحساب (رئيسي أو فرعي)
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}