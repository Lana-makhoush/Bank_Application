namespace Bank_Application.Models
{
    public class Loan
    {
        public int LoanId { get; set; }

        
        public int? AccountId { get; set; }
        public Account? Account { get; set; }

        public int? SubAccountId { get; set; }
        public SubAccount? SubAccount { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public decimal? RemainingAmount { get; set; }

        public int DurationInMonths { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime NextPaymentDate { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
