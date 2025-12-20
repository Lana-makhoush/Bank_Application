using Bank_Application.Models;

namespace Bank_Application.Repositories
{
    public interface IRecommendationRepository
    {
        Task AddAsync(Recommendation recommendation);
        Task<bool> ExistsAsync(int clientId, string message);
        Task<List<object>> GetByClientIdAsync(int clientId);
    }
}
