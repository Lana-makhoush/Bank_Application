namespace Bank_Application.DTOs
{
    public class TransactionReportItem
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = "";
        public string SenderAccount { get; set; } = "";
        public string ReceiverAccount { get; set; } = "";
        public decimal Amount { get; set; }
        public string ClientName { get; set; } = "";
        public string ApprovalStatus { get; set; } = "";
        public string ApprovedBy { get; set; } = "";
    }

}
