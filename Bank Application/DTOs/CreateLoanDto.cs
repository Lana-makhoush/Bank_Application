namespace Bank_Application.DTOs
{
    public class CreateLoanDto
    {
        public int? AccountId { get; set; }
        public int? SubAccountId { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public int DurationInMonths { get; set; }
    }

}
