using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Strategies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank_Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ITransactionLogRepository _transactionRepo;
        private readonly IRecommendationRepository _recommendationRepo;
        private readonly List<IRecommendationStrategy> _strategies;

        public RecommendationService(
            ITransactionLogRepository transactionRepo,
            IRecommendationRepository recommendationRepo)
        {
            _transactionRepo = transactionRepo;
            _recommendationRepo = recommendationRepo;

            _strategies = new()
            {
                new SavingsRecommendationStrategy(),
                new PremiumRecommendationStrategy()
            };
        }

        public async Task GenerateAsync(int clientId)
        {
            var transactions = await _transactionRepo.GetByClientIdAsync(clientId);

            foreach (var strategy in _strategies)
            {
                var message = strategy.Generate(transactions);

                if (!string.IsNullOrEmpty(message))
                {
                    if (!await _recommendationRepo.ExistsAsync(clientId, message))
                    {
                        await _recommendationRepo.AddAsync(new Recommendation
                        {
                            ClientId = clientId,
                            Message = message
                        });
                    }
                }
            }
        }

        public async Task<object> GetClientRecommendationsAsync(int clientId)
        {
            return await _recommendationRepo.GetByClientIdAsync(clientId);
        }
    }
}
