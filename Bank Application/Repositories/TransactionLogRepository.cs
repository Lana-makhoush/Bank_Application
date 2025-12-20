using Bank_Application.Data;
using Bank_Application.Models;

namespace Bank_Application.Repositories
{
    public class TransactionLogRepository : ITransactionLogRepository
    {
        private readonly AppDbContext _context;

        public TransactionLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TransactionLog log)
        {
            _context.TransactionLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }

}
