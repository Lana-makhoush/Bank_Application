namespace Bank_Application.DTOs
{
    public class PendingApprovalDto
    {
        public int ApprovalId { get; set; }
        public int TransactionId { get; set; }
        public int? TransactionTypeId { get; set; }
        public decimal Amount { get; set; }

        public int? SenderAccountId { get; set; }
        public int? ReceiverAccountId { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
