using Bank_Application.Models;
using System.Collections.Generic;

namespace Bank_Application.Strategies
{
    public class PremiumRecommendationStrategy : IRecommendationStrategy
    {
        public string? Generate(List<TransactionLog> transactions)
        {
            if (transactions.Count >= 30)
                return "نوصي بالترقية إلى حساب Premium.";

            return null;
        }
    }
}
