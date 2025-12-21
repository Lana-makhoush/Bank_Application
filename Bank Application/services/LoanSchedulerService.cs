using Bank_Application.Data;
using Bank_Application.design_pattern.Observe;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.services
{
    public class LoanSchedulerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LoanSchedulerService> _logger;
       

        public LoanSchedulerService(
     IServiceScopeFactory scopeFactory,
     ILogger<LoanSchedulerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
           
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                var notifier = scope.ServiceProvider
                    .GetRequiredService<ITransactionSubject>();
                try
                {
                    await ProcessLoansAsync(notifier);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing scheduled loans");
                }

                //await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);


            }
        }
        

        private async Task ProcessLoansAsync(
        ITransactionSubject notifier)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var schedules = await context.ScheduledTransactions
                .Include(s => s.Loan)
                .Where(s =>
               s.IsActive == true &&
                s.NextExecutionDate != null &&
                s.NextExecutionDate <= DateTime.Now)
                .ToListAsync();

            foreach (var s in schedules)
            {
                var loan = s.Loan;

                decimal? balance = null;
                int? clientId = null;

                if (s.SubAccountId != null)
                {
                    var sub = await context.SubAccounts.FindAsync(s.SubAccountId);
                    if (sub == null || sub.Balance < s.Amount) continue;

                    sub.Balance -= s.Amount;
                    balance = sub.Balance;
                    clientId = sub.ParentAccount!.AccountId;
                }
                else
                {
                    var ca = await context.ClientAccounts
                        .FirstOrDefaultAsync(c => c.AccountId == s.AccountId);

                    if (ca == null || ca.Balance < s.Amount) continue;

                    ca.Balance -= s.Amount;
                    clientId = ca.ClientId;
                }

                loan.RemainingAmount -= s.Amount;
                s.NextExecutionDate = s.NextExecutionDate.AddMonths(1);
                
                context.TransactionLogs.Add(new TransactionLog
                {
                    TransactionTypeId = (int)TransactionType.Withdrawal,
                    SenderAccountId = s.SubAccountId ?? s.AccountId,
                    Amount = s.Amount,
                    Description = "قسط قرض",
                    ClientId = clientId,
                    TransactionDate = DateTime.Now
                });
                await notifier.NotifyApprovedTransactionAsync(clientId, "تم سحب مبلغ قرض هذا الشهر ");
                if (loan.RemainingAmount <= 0)
                {
                    loan.IsActive = false;
                    s.IsActive = false;
                }
            }

            await context.SaveChangesAsync();
        }
    }

}
