using Bank_Application.Data;
using Bank_Application.Models;
using Bank_Application.DesignPatterns.State;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bank_Application.Repositories
{
    public class ClientAccountRepository : IClientAccountRepository
    {
        private readonly AppDbContext _context;

        public ClientAccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClientAccount?> GetByIdAsync(int id)
        {
            return await _context.ClientAccounts
                .Include(ca => ca.Account)
                .FirstOrDefaultAsync(ca => ca.Id == id);
        }

        public async Task<(bool Success, string Message)> CloseAccountAsync(int id)
        {
            var account = await GetByIdAsync(id);
            if (account == null)
                return (false, "الحساب غير موجود");

            var hasActiveTransactions = await _context.ScheduledTransactions
                .AnyAsync(st => st.AccountId == account.AccountId && st.IsActive == true);

            if (hasActiveTransactions)
                return (false, "لا يمكن إغلاق الحساب لأنه يحتوي على معاملات مجدولة نشطة.");

            var context = new AccountContext(account);
            context.Close();

            if (account.Account != null)
                account.Account.AccountStatusId = 4;

            await _context.SaveChangesAsync();
            return (true, "تم غلق الحساب الرئيسي وتصفير الرصيد");
        }

        public async Task<(bool Success, string Message)> ActivateAccountAsync(int id)
        {
            var account = await GetByIdAsync(id);
            if (account == null)
                return (false, "الحساب غير موجود");

            var context = new AccountContext(account);
            context.Activate(); 

            await _context.SaveChangesAsync();
            return (true, "تم تنشيط الحساب الرئيسي");
        }
        public async Task<(bool Success, string Message)> SuspendAccountAsync(int id)
        {
            var account = await GetByIdAsync(id);
            if (account == null)
                return (false, "الحساب غير موجود");

            if (account.Account != null)
                account.Account.AccountStatusId = 3; 

            await _context.SaveChangesAsync();
            return (true, "تم تعليق الحساب الرئيسي");
        }

        public async Task<(bool Success, string Message)> FreezeAccountAsync(int id)
        {
            var account = await GetByIdAsync(id);
            if (account == null)
                return (false, "الحساب غير موجود");

            if (account.Account != null)
                account.Account.AccountStatusId = 2;

            await _context.SaveChangesAsync();
            return (true, "تم تجميد الحساب الرئيسي");
        }

    }

}

