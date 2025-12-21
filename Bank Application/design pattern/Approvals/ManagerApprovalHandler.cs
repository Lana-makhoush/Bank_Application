using Bank_Application.Approvals;
using Bank_Application.Data;

public class ManagerApprovalHandler : ApprovalHandlerBase
{
    private readonly AppDbContext _context;

    public ManagerApprovalHandler(AppDbContext context)
    {
        _context = context;
    }

    public override async Task HandleAsync(ApprovalContext context)
    {
        context.NeedsApproval = true;

        var approval = new TransactionApproval
        {
            TransactionLogId = context.Transaction.TransactionLogId,
            Status = "Pending",
            RequiredRole = "Manager"
        };

        _context.TransactionApprovals.Add(approval);
        await _context.SaveChangesAsync();

        // نوقف السلسلة، بانتظار موافقة
    }
}
