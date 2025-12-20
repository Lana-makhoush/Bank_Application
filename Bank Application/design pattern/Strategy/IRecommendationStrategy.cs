using Bank_Application.Models;
using System.Collections.Generic;

namespace Bank_Application.Strategies
{
    public interface IRecommendationStrategy
    {
        string? Generate(List<TransactionLog> transactions);
    }
}
