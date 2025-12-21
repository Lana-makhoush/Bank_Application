using Bank_Application.Data;
using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.services
{
    public class LoanService
    {
        private readonly AppDbContext _context;
        private readonly IClientAccountTransactionRepository _clientAccountRepo;

        public LoanService(AppDbContext context, IClientAccountTransactionRepository clientAccountRepo)
        {
            _context = context;
            _clientAccountRepo = clientAccountRepo;
        }

        public async Task<ServiceResult> CreateLoanAsync(CreateLoanDto dto)
        {
            if (dto.AccountId == null && dto.SubAccountId == null)
                return ServiceResult.Fail("يجب تحديد حساب رئيسي أو فرعي");

            if (dto.AccountId != null && dto.SubAccountId != null)
                return ServiceResult.Fail("لا يمكن ربط القرض بحسابين");

            var strategy = _context.Database.CreateExecutionStrategy();
            ServiceResult result = ServiceResult.Fail("فشل تنفيذ العملية");

            await strategy.ExecuteAsync(async () =>
            {
                using var trx = await _context.Database.BeginTransactionAsync();

              
                if (dto.SubAccountId != null)
                {
                    var sub = await _context.SubAccounts
                        .FirstOrDefaultAsync(s => s.SubAccountId == dto.SubAccountId);

                    if (sub == null)
                    {
                        result = ServiceResult.Fail("الحساب الفرعي غير موجود");
                        return;
                    }

                    sub.Balance += dto.TotalAmount;
                }
                else
                {
                    var clientAccount = await _clientAccountRepo
                        .GetByAccountIdAsync(dto.AccountId!.Value);

                    if (clientAccount == null)
                    {
                        result = ServiceResult.Fail("الحساب الرئيسي غير مرتبط بعميل");
                        return;
                    }

                    clientAccount.Balance += dto.TotalAmount;
                }

                
                var loan = new Loan
                {
                    AccountId = dto.AccountId,
                    SubAccountId = dto.SubAccountId,
                    TotalAmount = dto.TotalAmount,
                    MonthlyInstallment = dto.MonthlyInstallment,
                    RemainingAmount = dto.TotalAmount,
                    DurationInMonths = dto.DurationInMonths,
                    StartDate = DateTime.Now,
                   // NextPaymentDate = DateTime.Now.AddSeconds(10),
                    NextPaymentDate = DateTime.Now.AddMonths(1),
                    IsActive = true
                };

                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();

                
                var schedule = new ScheduledTransaction
                {
                    LoanId = loan.LoanId,
                    AccountId = dto.AccountId,
                    SubAccountId = dto.SubAccountId,
                    Amount = dto.MonthlyInstallment,
                    NextExecutionDate = loan.NextPaymentDate,
                    RecurrenceType = "Monthly",
                    IsActive = true
                };

                _context.ScheduledTransactions.Add(schedule);
                await _context.SaveChangesAsync();

                await trx.CommitAsync();

                result = ServiceResult.Ok("تم إنشاء القرض بنجاح");
            });

            return result;
        }

    }

}
