using Bank_Application.Models;

namespace Bank_Application.Repositories
{
    public interface IClientAccountTransactionRepository
    {
        Task<ClientAccount?> GetByAccountIdAsync(int accountId);
    }
}
