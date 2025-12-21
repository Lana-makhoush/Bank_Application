using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.Repositories
{
    public class ClientAccountTransactionRepository: IClientAccountTransactionRepository
    {
      
      
            private readonly AppDbContext _context;

            public ClientAccountTransactionRepository(AppDbContext context)
            {
                _context = context;
            }

            public Task<ClientAccount?> GetByAccountIdAsync(int accountId)
                => _context.ClientAccounts.FirstOrDefaultAsync(ca => ca.AccountId == accountId);
        

    }
}
