using Bank_Application.DTOs;

namespace Bank_Application.services
{
    public interface ITransactionApprovalService
    {
        Task<ServiceResult> ApproveAsync(int approvalId, int managerEmployeeId);
        Task<ServiceResult> RejectAsync(int approvalId, int managerEmployeeId);

        Task<List<PendingApprovalDto>> GetPendingApprovalsAsync();
    }

}
