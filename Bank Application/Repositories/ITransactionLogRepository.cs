using Bank_Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank_Application.Repositories
{
    public interface ITransactionLogRepository
    {
        Task<List<TransactionLog>> GetByClientIdAsync(int clientId);
    }

   
}
