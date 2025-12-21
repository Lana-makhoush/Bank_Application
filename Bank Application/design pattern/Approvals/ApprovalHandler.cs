
using Bank_Application.Approvals;

public abstract class ApprovalHandlerBase : IApprovalHandler
{
    private IApprovalHandler? _next;

    public IApprovalHandler SetNext(IApprovalHandler handler)
    {
        _next = handler;
        return handler;
    }

    public virtual async Task HandleAsync(ApprovalContext context)
    {
        if (_next != null)
            await _next.HandleAsync(context);
    }
}

