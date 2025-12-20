using Bank_Application.Approvals;

public class SmallAmountApprovalHandler : ApprovalHandlerBase
{
    public override async Task HandleAsync(ApprovalContext context)
    {
        if (context.Amount <= context.DailyLimit * 0.75m)
        {
            context.IsApproved = true;
            return; // stop chain
        }

        await base.HandleAsync(context);
    }
}
