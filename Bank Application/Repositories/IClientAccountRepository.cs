using Bank_Application.Models;
using System.Threading.Tasks;

namespace Bank_Application.Repositories
{
    public interface IClientAccountRepository
    {
        Task<ClientAccount> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CloseAccountAsync(int id);
        Task<(bool Success, string Message)> ActivateAccountAsync(int id);
        Task<(bool Success, string Message)> SuspendAccountAsync(int id);

        Task<(bool Success, string Message)> FreezeAccountAsync(int id);

    }
}
