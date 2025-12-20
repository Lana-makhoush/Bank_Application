using Bank_Application.Approvals;
using Bank_Application.Data;
using Bank_Application.design_pattern.Observe;
using Bank_Application.DTOs;
using Bank_Application.Hubs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Bank_Application.Services
{
    public class TransactionService:ITransactionService
    {
        private readonly AppDbContext _context;
        private readonly IAccountResolverService _resolver;
        private readonly ITransactionSubject _notifier;
         private readonly IClientAccountTransactionRepository _clientAccountRepo;
    private readonly ITransactionLogRepository _logRepo;
        private readonly IAccountTransactionRepository _accountTransactionRepo;
        public TransactionService(
            AppDbContext context,
            IAccountResolverService resolver,
            IClientAccountTransactionRepository clientAccountRepo,
        ITransactionLogRepository logRepo,
        IAccountTransactionRepository accountRepo,
            ITransactionSubject notifier)
        {
            _context = context;
            _resolver = resolver;
            _notifier = notifier;
            _clientAccountRepo = clientAccountRepo;
            _logRepo = logRepo;
            _accountTransactionRepo = accountRepo;
            // تسجيل الـ observer
            _notifier.Register(new NotificationObserver(
                context.GetService<IHubContext<NotificationHub>>()));
        }
        public async Task<ServiceResult> DepositAsync(DepositDto dto)
        {
            if (dto.Amount <= 0)
                return ServiceResult.Fail("المبلغ غير صالح");

            var strategy = _context.Database.CreateExecutionStrategy();
            ServiceResult result = ServiceResult.Fail("فشل تنفيذ العملية");

            await strategy.ExecuteAsync(async () =>
            {
                using var trx = await _context.Database.BeginTransactionAsync();

                var (main, sub) = await _resolver.FindAccountAsync(dto.AccountNumber);
                if (main == null && sub == null)
                {
                    result = ServiceResult.Fail("الحساب غير موجود");
                    return;
                }

                int? clientId;

                if (sub != null)
                {
                    result = EnsureAccountIsActive(sub.SubAccountStatusId);
                    if (!result.Success) return;

                    sub.Balance += dto.Amount;

                    clientId = (await _clientAccountRepo
                        .GetByAccountIdAsync(sub.ParentAccountId!.Value))?.ClientId;
                }
                else
                {
                    result = EnsureAccountIsActive(main!.AccountStatusId);
                    if (!result.Success) return;

                    var clientAccount = await _clientAccountRepo
                        .GetByAccountIdAsync(main.AccountId);

                    if (clientAccount == null)
                    {
                        result = ServiceResult.Fail("لا يوجد عميل مرتبط بهذا الحساب");
                        return;
                    }

                    clientAccount.Balance += dto.Amount;
                    clientId = clientAccount.ClientId;
                }

                var log = CreateDepositLog(dto, clientId);
                await _logRepo.AddAsync(log);

                await trx.CommitAsync();
                await _notifier.NotifyDepositAsync(log);

                result = ServiceResult.Ok("تم الإيداع بنجاح");
            });

            return result;
        }

        public async Task<ServiceResult> WithdrawalAsync(DepositDto dto)
        {
            if (dto.Amount <= 0)
                return ServiceResult.Fail("المبلغ غير صالح");

            var strategy = _context.Database.CreateExecutionStrategy();
            ServiceResult result = ServiceResult.Fail("فشل العملية");

            await strategy.ExecuteAsync(async () =>
            {
                using var trx = await _context.Database.BeginTransactionAsync();

              
                var (main, sub) = await _resolver.FindAccountAsync(dto.AccountNumber);
                if (main == null && sub == null)
                {
                    result = ServiceResult.Fail("الحساب غير موجود");
                    return;
                }

                int? clientId;
                decimal dailyLimit;
                decimal currentBalance;

               
                if (sub != null)
                {
                    result = EnsureAccountIsActive(sub.SubAccountStatusId);
                    if (!result.Success) return;

                    dailyLimit = sub.DailyWithdrawalLimit ?? 0;
                    currentBalance = sub.Balance ?? 0;

                    result = EnsureDailyWithdrawalLimit(dailyLimit, dto.Amount);
                    if (!result.Success) return;

                    result = EnsureSufficientBalance(currentBalance, dto.Amount);
                    if (!result.Success) return;

                    

                    clientId = (await _clientAccountRepo
                        .GetByAccountIdAsync(sub.ParentAccountId!.Value))?.ClientId;
                }
                else
                {
                    result = EnsureAccountIsActive(main!.AccountStatusId);
                    if (!result.Success) return;

                    var clientAccount = await _clientAccountRepo.GetByAccountIdAsync(main.AccountId);
                    if (clientAccount == null)
                    {
                        result = ServiceResult.Fail("لا يوجد عميل مرتبط بالحساب");
                        return;
                    }

                    dailyLimit = (await _accountTransactionRepo
                        .GetMainWithdrawalLimitAsync(main.AccountId)) ?? 0;

                    currentBalance = clientAccount.Balance ?? 0;

                    result = EnsureDailyWithdrawalLimit(dailyLimit, dto.Amount);
                    if (!result.Success) return;

                    result = EnsureSufficientBalance(currentBalance, dto.Amount);
                    if (!result.Success) return;

                    
                    clientId = clientAccount.ClientId;
                }

                
                var log = CreateWithdrawalLog(dto, clientId);
                await _logRepo.AddAsync(log);
                await _context.SaveChangesAsync();

                // Chain of Responsibility 
                var approvalContext = new ApprovalContext
                {
                    Transaction = log,
                    DailyLimit = dailyLimit
                };

                var smallAmountHandler = new SmallAmountApprovalHandler();
                var managerApprovalHandler = new ManagerApprovalHandler(_context);

                smallAmountHandler.SetNext(managerApprovalHandler);
                await smallAmountHandler.HandleAsync(approvalContext);

                //  إذا العملية تحتاج موافقة مدير
                if (approvalContext.NeedsApproval)
                {
                    await trx.CommitAsync();
                    result = ServiceResult.Ok("عملية السحب بانتظار موافقة المدير");
                    await _notifier.NotifyManagerAsync(log);
                    
                    return;
                }
                if (sub != null)
                    sub.Balance -= dto.Amount;
                else
                {
                    var clientAccount = await _clientAccountRepo.GetByAccountIdAsync(main.AccountId);
                    clientAccount.Balance -= dto.Amount;
                }

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
                await _notifier.NotifyWithdrawalAsync(log);

                result = ServiceResult.Ok("تم السحب بنجاح");
            });

            return result;
        }
        public async Task<ServiceResult> TransferAsync(TransferDto dto)
        {
            if (dto.Amount <= 0)
                return ServiceResult.Fail("المبلغ غير صالح");

            if (dto.FromAccountNumber == dto.ToAccountNumber)
                return ServiceResult.Fail("لا يمكن التحويل لنفس الحساب");

            var strategy = _context.Database.CreateExecutionStrategy();
            ServiceResult result = ServiceResult.Fail("فشل عملية التحويل");

            await strategy.ExecuteAsync(async () =>
            {
                using var trx = await _context.Database.BeginTransactionAsync();

               
                var (fromMain, fromSub) = await _resolver.FindAccountAsync(dto.FromAccountNumber);
                var (toMain, toSub) = await _resolver.FindAccountAsync(dto.ToAccountNumber);

                if ((fromMain == null && fromSub == null) || (toMain == null && toSub == null))
                {
                    result = ServiceResult.Fail("أحد الحسابات غير موجود");
                    return;
                }

                
                decimal dailyLimit;
                decimal senderBalance;
                int? clientId;
                int? receiverClientId;
                if (fromSub != null)
                {
                    result = EnsureAccountIsActive(fromSub.SubAccountStatusId);
                    if (!result.Success) return;

                    dailyLimit = fromSub.DailyWithdrawalLimit ?? 0;
                    senderBalance = fromSub.Balance ?? 0;

                    clientId = (await _clientAccountRepo
                        .GetByAccountIdAsync(fromSub.ParentAccountId!.Value))?.ClientId;
                }
                else
                {
                    result = EnsureAccountIsActive(fromMain!.AccountStatusId);
                    if (!result.Success) return;

                    var clientAccount = await _clientAccountRepo.GetByAccountIdAsync(fromMain.AccountId);
                    if (clientAccount == null)
                    {
                        result = ServiceResult.Fail("لا يوجد عميل مرتبط بالحساب المرسل");
                        return;
                    }

                    dailyLimit = (await _accountTransactionRepo
                        .GetMainWithdrawalLimitAsync(fromMain.AccountId)) ?? 0;

                    senderBalance = clientAccount.Balance ?? 0;
                    clientId = clientAccount.ClientId;
                }

                result = EnsureDailyWithdrawalLimit(dailyLimit, dto.Amount);
                if (!result.Success) return;

                result = EnsureSufficientBalance(senderBalance, dto.Amount);
                if (!result.Success) return;


                var log = CreateTransferLog(dto, clientId);

                await _logRepo.AddAsync(log);
                await _context.SaveChangesAsync();

                
                var approvalContext = new ApprovalContext
                {
                    Transaction = log,
                    DailyLimit = dailyLimit
                };

                var smallHandler = new SmallAmountApprovalHandler();
                var managerHandler = new ManagerApprovalHandler(_context);

                smallHandler.SetNext(managerHandler);
                await smallHandler.HandleAsync(approvalContext);

              
                if (approvalContext.NeedsApproval)
                {
                    await trx.CommitAsync();
                    await _notifier.NotifyManagerAsync(log);
                    result = ServiceResult.Ok("التحويل بانتظار موافقة المدير");
                    return;
                }

              
                if (fromSub != null)
                    fromSub.Balance -= dto.Amount;
                else
                {
                    var clientAccount = await _clientAccountRepo.GetByAccountIdAsync(fromMain!.AccountId);
                    clientAccount!.Balance -= dto.Amount;
                }

                if (toSub != null)
                {
                    toSub.Balance += dto.Amount;
                     receiverClientId = (await _clientAccountRepo
                  .GetByAccountIdAsync(toSub.ParentAccountId!.Value))?.ClientId;
                   
                }


                else
                {
                    var clientAccount = await _clientAccountRepo.GetByAccountIdAsync(toMain!.AccountId);
                    clientAccount!.Balance += dto.Amount;
                    var receiverClientAccount =
                    await _clientAccountRepo.GetByAccountIdAsync(toMain!.AccountId);
                    receiverClientId = receiverClientAccount!.ClientId;
                }
                await _context.SaveChangesAsync();
               
                await _notifier.NotifyTransferToAsync(receiverClientId,dto.Amount);
               
                await trx.CommitAsync();

                await _notifier.NotifyTransferAsync(log);
                
                result = ServiceResult.Ok("تم التحويل بنجاح");
            });

            return result;
        }


        private ServiceResult EnsureAccountIsActive(int? statusId)
        {
            if (statusId != 1)
                return ServiceResult.Fail("الحساب غير نشط ولا يمكن تنفيذ العملية");

            return ServiceResult.Ok("نشط ");
        }

        private ServiceResult EnsureSufficientBalance(decimal? balance, decimal amount)
        {
            if (balance == null || balance < amount)
                return ServiceResult.Fail("رصيدك أقل من مبلغ السحب");

            return ServiceResult.Ok();
        }
        private TransactionLog CreateWithdrawalLog(DepositDto dto, int? clientId)
        {
            return new TransactionLog
            {
                TransactionTypeId = 2, // Withdrawal
                SenderAccountId = dto.AccountNumber,
                Amount = dto.Amount,
                Description = dto.Description ?? "سحب",
                ClientId = clientId,
                TransactionDate = DateTime.Now
            };
        }
        private TransactionLog CreateDepositLog(DepositDto dto, int? clientId)
        {
            return new TransactionLog
            {
                TransactionTypeId = 1, // Deposit
                ReceiverAccountId = dto.AccountNumber,
                Amount = dto.Amount,
                Description = dto.Description ?? "إيداع",
                ClientId = clientId,
                TransactionDate = DateTime.Now
            };
        }
        private TransactionLog CreateTransferLog(TransferDto dto, int? clientId)
        {
            return new TransactionLog
            {
                TransactionTypeId = 3, // Transfer
                SenderAccountId = dto.FromAccountNumber,
                ReceiverAccountId = dto.ToAccountNumber,
                Amount = dto.Amount,
                Description = dto.Description ?? "تحويل",
                ClientId = clientId,
                TransactionDate = DateTime.Now
            };
        }
        private ServiceResult EnsureDailyWithdrawalLimit(decimal? limit, decimal amount)
        {
            if (limit == null)
                return ServiceResult.Fail("لا يوجد حد يومي للسحب");

            if (amount > limit)
                return ServiceResult.Fail("المبلغ يتجاوز الحد اليومي للسحب");

            return ServiceResult.Ok();
        }


    }
}
