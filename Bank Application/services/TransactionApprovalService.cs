using Bank_Application.Data;
using Bank_Application.design_pattern.Observe;
using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.services
{
    public class TransactionApprovalService : ITransactionApprovalService
    {
        private readonly AppDbContext _context;
        private readonly IAccountResolverService _resolver;
        private readonly IClientAccountTransactionRepository _clientAccountRepo;
        private readonly ITransactionSubject _notifier;

        public TransactionApprovalService(
            AppDbContext context,
            IAccountResolverService resolver,
            IClientAccountTransactionRepository clientAccountRepo,
            ITransactionSubject notifier)
        {
            _context = context;
            _resolver = resolver;
            _clientAccountRepo = clientAccountRepo;
            _notifier = notifier;
        }
        public async Task<List<PendingApprovalDto>> GetPendingApprovalsAsync()
        {
            return await _context.TransactionApprovals
                .Include(a => a.TransactionLog)
                .Where(a => a.Status == "Pending")
                .Select(a => new PendingApprovalDto
                {
                    ApprovalId = a.Id,
                    TransactionId = a.TransactionLogId,
                    TransactionTypeId = a.TransactionLog.TransactionTypeId,
                    Amount = a.TransactionLog.Amount!.Value,
                    SenderAccountId = a.TransactionLog.SenderAccountId,
                    ReceiverAccountId = a.TransactionLog.ReceiverAccountId,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();
        }
       public async Task<ServiceResult> ApproveAsync(int approvalId, int managerEmployeeId)
{
    var strategy = _context.Database.CreateExecutionStrategy();
    ServiceResult result = ServiceResult.Fail("فشل تنفيذ العملية");

    await strategy.ExecuteAsync(async () =>
    {
        var approval = await _context.TransactionApprovals
            .Include(a => a.TransactionLog)
            .FirstOrDefaultAsync(a => a.Id == approvalId);

        if (approval == null || approval.Status != "Pending")
        {
            result = ServiceResult.Fail("الطلب غير صالح");
            return;
        }

        using var trx = await _context.Database.BeginTransactionAsync();

        var log = approval.TransactionLog;

        switch ((TransactionType)log.TransactionTypeId)
        {
            case TransactionType.Withdrawal:
                await ExecuteWithdrawalAsync(log);
                break;

            case TransactionType.Transfer:
                await ExecuteTransferAsync(log);
                break;

            default:
                result = ServiceResult.Fail("نوع العملية غير مدعوم");
                return;
        }

        approval.Status = "Approved";
        approval.ApprovedAt = DateTime.Now;
        approval.ApprovedByEmployeeId = managerEmployeeId;

        await _context.SaveChangesAsync();
        await trx.CommitAsync();
        await _notifier.NotifyApprovedTransactionAsync(approval.TransactionLog.ClientId, "تمت الموافقة على العملية"
    );
        result = ServiceResult.Ok("تمت الموافقة وتنفيذ العملية");
    });

    
    
   

    return result;
}

        public async Task<ServiceResult> RejectAsync(int approvalId, int managerEmployeeId)
        {
            var approval = await _context.TransactionApprovals
                .Include(a => a.TransactionLog)
                .FirstOrDefaultAsync(a => a.Id == approvalId);

            if (approval == null || approval.Status != "Pending")
                return ServiceResult.Fail("الطلب غير صالح");

            approval.Status = "Rejected";
            approval.ApprovedAt = DateTime.Now;
            approval.ApprovedByEmployeeId = managerEmployeeId;

            await _context.SaveChangesAsync();

            await _notifier.NotifyApprovedTransactionAsync(approval.TransactionLog.ClientId,"تم رفص العملية ");

            return ServiceResult.Ok("تم رفض العملية");
        }
        private async Task ExecuteWithdrawalAsync(TransactionLog log)
        {
            var (main, sub) = await _resolver.FindAccountAsync(log.SenderAccountId!.Value);

            if (sub != null)
            {
                sub.Balance -= log.Amount!.Value;
            }
            else
            {
                var ca = await _clientAccountRepo
                    .GetByAccountIdAsync(main!.AccountId);

                ca!.Balance -= log.Amount!.Value;
            }
            await _notifier.NotifyWithdrawalAsync(log);
        }
        private async Task ExecuteTransferAsync(TransactionLog log)
        {
            int? receiverClientId;
            var (fromMain, fromSub) =
                await _resolver.FindAccountAsync(log.SenderAccountId!.Value);

            var (toMain, toSub) =
                await _resolver.FindAccountAsync(log.ReceiverAccountId!.Value);

            if (fromSub != null)
                fromSub.Balance -= log.Amount!.Value;
            else
            {
                var ca = await _clientAccountRepo
                    .GetByAccountIdAsync(fromMain!.AccountId);
                ca!.Balance -= log.Amount!.Value;
            }

            if (toSub != null) { 
                toSub.Balance += log.Amount!.Value;
            receiverClientId = (await _clientAccountRepo
                 .GetByAccountIdAsync(toSub.ParentAccountId!.Value))?.ClientId; }
            else
            {
                var ca = await _clientAccountRepo
                    .GetByAccountIdAsync(toMain!.AccountId);
                ca!.Balance += log.Amount!.Value;
                var receiverClientAccount =
                    await _clientAccountRepo.GetByAccountIdAsync(toMain!.AccountId);
                receiverClientId = receiverClientAccount!.ClientId;
            }
            await _notifier.NotifyTransferAsync(log);

            await _notifier.NotifyTransferToAsync(receiverClientId, log.Amount);
        }

    }
    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal = 2,
        Transfer = 3
    }
}

