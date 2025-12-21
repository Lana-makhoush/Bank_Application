namespace Bank_Application.DTOs
{
    public class WithdrawDto
    {
        public int AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

}
