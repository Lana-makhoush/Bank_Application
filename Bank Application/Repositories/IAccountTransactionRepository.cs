using Bank_Application.Models;

namespace Bank_Application.Repositories
{
   
    public interface IAccountTransactionRepository
    {
        Task<Account?> GetMainAsync(int accountId);
        Task<SubAccount?> GetSubAsync(int accountId);
        Task<decimal?> GetMainWithdrawalLimitAsync(int accountId);
    }

}
