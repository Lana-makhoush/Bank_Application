using Bank_Application.Models;

namespace Bank_Application.Repositories
{
    public interface ITransactionLogRepository
    {
        Task AddAsync(TransactionLog log);
    }

}
