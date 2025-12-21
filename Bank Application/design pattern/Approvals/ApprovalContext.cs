using Bank_Application.Models;
namespace Bank_Application.Approvals
{
    public class ApprovalContext
    {
       
            public TransactionLog Transaction { get; set; } = null!;
            public decimal Amount => Transaction.Amount ?? 0;
            public decimal DailyLimit { get; set; }

            public bool NeedsApproval { get; set; }
            public bool IsApproved { get; set; }
        

    }
}