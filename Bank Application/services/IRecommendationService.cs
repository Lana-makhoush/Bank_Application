using System.Threading.Tasks;

namespace Bank_Application.Services
{
    public interface IRecommendationService
    {
        Task GenerateAsync(int clientId);
        Task<List<object>> GetClientRecommendationsAsync(int clientId);
    }
}
