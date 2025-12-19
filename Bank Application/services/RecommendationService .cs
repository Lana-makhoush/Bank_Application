using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Services;
using Bank_Application.Strategies;

public class RecommendationService : IRecommendationService
{
    private readonly ITransactionLogRepository _transactionRepo;
    private readonly IRecommendationRepository _recommendationRepo;
    private readonly IEnumerable<IRecommendationStrategy> _strategies;

    public RecommendationService(
        ITransactionLogRepository transactionRepo,
        IRecommendationRepository recommendationRepo,
        IEnumerable<IRecommendationStrategy> strategies)
    {
        _transactionRepo = transactionRepo;
        _recommendationRepo = recommendationRepo;
        _strategies = strategies;
    }

    public async Task GenerateAsync(int clientId)
    {
        var transactions = await _transactionRepo.GetByClientIdAsync(clientId);

        foreach (var strategy in _strategies)
        {
            var message = strategy.Generate(transactions);

            if (!string.IsNullOrEmpty(message) &&
                !await _recommendationRepo.ExistsAsync(clientId, message))
            {
                await _recommendationRepo.AddAsync(new Recommendation
                {
                    ClientId = clientId,
                    Message = message
                });
            }
        }
    }

    public async Task<List<object>> GetClientRecommendationsAsync(int clientId)
    {
        return await _recommendationRepo.GetByClientIdAsync(clientId);
    }

}
