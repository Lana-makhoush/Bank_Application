using Bank_Application.Approvals;

public interface IApprovalHandler
{
    IApprovalHandler SetNext(IApprovalHandler handler);
    Task HandleAsync(ApprovalContext context);
}
