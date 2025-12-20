using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.Repositories
{
    public class AccountTransactionRepository : IAccountTransactionRepository
    {
        private readonly AppDbContext _context;

        public AccountTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Account?> GetMainAsync(int accountId)
            => _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);

        public Task<SubAccount?> GetSubAsync(int accountId)
            => _context.SubAccounts.FirstOrDefaultAsync(s => s.SubAccountId == accountId);

        public async Task<decimal?> GetMainWithdrawalLimitAsync(int accountId)
        {
            var account = await _context.Accounts
                .Include(a => a.AccountType)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);

            return account?.AccountType?.DailyWithdrawalLimit;
        }
    }

}
