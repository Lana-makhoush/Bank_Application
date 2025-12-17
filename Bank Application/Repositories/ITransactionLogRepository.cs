using Bank_Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank_Application.Repositories
{
    public interface ITransactionLogRepository
    {
        Task<List<TransactionLog>> GetByClientIdAsync(int clientId);
    }

    public interface IRecommendationRepository
    {
        Task AddAsync(Recommendation recommendation);
        Task<bool> ExistsAsync(int clientId, string message);
        Task<List<Recommendation>> GetByClientIdAsync(int clientId);
    }
}
