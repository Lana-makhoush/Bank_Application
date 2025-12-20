using Bank_Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bank_Application.Strategies
{
    public class SavingsRecommendationStrategy : IRecommendationStrategy
    {
        public string? Generate(List<TransactionLog> transactions)
        {
            var total = transactions.Sum(t => t.Amount ?? 0);

            if (total > 10000)
                return "نوصي بفتح حساب توفير للاستفادة من الفوائد.";

            return null;
        }
    }
}
