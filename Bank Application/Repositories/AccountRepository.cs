using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ClientExists(int clientId)
        {
            return await _context.Clients.AnyAsync(c => c.ClientId == clientId);
        }

        public async Task<bool> AccountTypeExists(int accountTypeId)
        {
            return await _context.AccountTypes.AnyAsync(a => a.AccountTypeId == accountTypeId);
        }

        public async Task<Account> CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<ClientAccount> CreateClientAccount(ClientAccount clientAccount)
        {
            _context.ClientAccounts.Add(clientAccount);
            await _context.SaveChangesAsync();
            return clientAccount;
        }

        public async Task<Client?> GetClientById(int clientId)
        {
            return await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
        }

       
       
        public async Task<bool> ClientHasAccountType(int clientId, int accountTypeId)
        {
            return await _context.ClientAccounts
                .Include(ca => ca.Account)
                .AnyAsync(ca => ca.ClientId == clientId && ca.Account!.AccountTypeId == accountTypeId);
        }
        public async Task<ClientAccount?> UpdateAccount(int clientAccountId, int newAccountTypeId, decimal newBalance)
        {
            var clientAccount = await _context.ClientAccounts
                .Include(ca => ca.Client)
                .Include(ca => ca.Account)
                    .ThenInclude(a => a.AccountType)
                .FirstOrDefaultAsync(ca => ca.Id == clientAccountId);

            if (clientAccount == null)
                return null;

            clientAccount.Balance = newBalance;

            if (clientAccount.Account != null)
                clientAccount.Account.AccountTypeId = newAccountTypeId;

            await _context.SaveChangesAsync();

            clientAccount.Account = await _context.Accounts
                .Include(a => a.AccountType)
                .FirstOrDefaultAsync(a => a.AccountId == clientAccount.AccountId);

            return clientAccount;
        }

        public async Task<ClientAccount?> GetClientAccountById(int clientAccountId)
        {
            return await _context.ClientAccounts
                .Include(ca => ca.Client)
                .Include(ca => ca.Account)
                    .ThenInclude(a => a.AccountType)
                .FirstOrDefaultAsync(ca => ca.Id == clientAccountId); 
        }

        public async Task<List<ClientAccount>> GetAllClientAccounts()
        {
            return await _context.ClientAccounts
                .Include(ca => ca.Client)
                .Include(ca => ca.Account)
                    .ThenInclude(a => a.AccountType)
                .ToListAsync();
        }
        public async Task<Account?> GetAccountByIdAsync(int accountId)
        {
            return await _context.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.AccountStatus)
                .Include(a => a.SubAccounts)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
        public async Task<List<Feature>> GetFeaturesByAccountType(int accountTypeId)
        {
            return await _context.AccountTypeFeatures
                .Include(atf => atf.Feature)
                .Where(atf => atf.AccountTypeId == accountTypeId)
                .Select(atf => atf.Feature)
                .ToListAsync();
        }
        public Task<List<SubAccount>> GetAllSubAccounts()
        {
            return _context.SubAccounts
                .ToListAsync();
        }
        public async Task<AccountType?> GetAccountTypeById(int accountTypeId)
        {
            return await _context.AccountTypes
                .Include(at => at.AccountTypeFeatures)
                    .ThenInclude(atf => atf.Feature)
                .FirstOrDefaultAsync(at => at.AccountTypeId == accountTypeId);
        }




        public async Task UpdateClientAccountBalance(int clientAccountId, decimal newBalance)
        {
            var account = await _context.ClientAccounts
                .FirstOrDefaultAsync(a => a.Id == clientAccountId);

            if (account == null) return;

            account.Balance = newBalance;

            _context.Entry(account).Property(x => x.Balance).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubAccountBalance(int subAccountId, decimal newBalance)
        {
            var subAccount = await _context.SubAccounts
                .FirstOrDefaultAsync(s => s.SubAccountId == subAccountId);

            if (subAccount == null) return;

            subAccount.Balance = newBalance;

            _context.Entry(subAccount).Property(x => x.Balance).IsModified = true;
            await _context.SaveChangesAsync();
        }
        public async Task<List<Account>> GetAccountsWithSubAccountsByClientIdAsync(int clientId)
        {
            return await _context.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.SubAccounts) 
                .Include(a => a.ClientAccounts)
                .Where(a => a.ClientAccounts.Any(ca => ca.ClientId == clientId))
                .ToListAsync();
        }
        public async Task<ClientAccount?> GetClientAccountByAccountIdAsync(int accountId)
        {
            return await _context.ClientAccounts
                .FirstOrDefaultAsync(ca => ca.AccountId == accountId);
        }

    }
}
