using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.Services
{
    public class AccountResolverService: IAccountResolverService
    {
        private readonly AppDbContext _context;

        public AccountResolverService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(Account? main, SubAccount? sub)> FindAccountAsync(int accountNumber)
        {
            var main = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == accountNumber);

            if (main != null)
                return (main, null);

            var sub = await _context.SubAccounts
                .FirstOrDefaultAsync(s => s.SubAccountId == accountNumber);

            return (null, sub);
        }

        public async Task<decimal> FindLimitWithdrawalAsync(int accountNumber)
        {
            var main = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == accountNumber);

            var balancelimit = await _context.AccountTypes.FirstOrDefaultAsync(a => a.AccountTypeId == main!.AccountTypeId);

            decimal balance =(decimal) balancelimit!.DailyWithdrawalLimit!;
            return balance;
        }



    }

}
