using Bank_Application.Models;

namespace Bank_Application.Services
{
    public interface IAccountResolverService
    {
        Task<(Account? main, SubAccount? sub)> FindAccountAsync(int accountNumber);
        Task<decimal> FindLimitWithdrawalAsync(int accountNumber);
    }
}
