using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank_Application.Repositories
{
    public class TransactionLogRepository : ITransactionLogRepository
    {
        private readonly AppDbContext _context;

        public TransactionLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionLog>> GetByClientIdAsync(int clientId)
        {
            return await _context.TransactionLogs
                .Where(t => t.ClientId == clientId)
            .ToListAsync();
        }
        public async Task AddAsync(TransactionLog log)
        {
            _context.TransactionLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }

   
}
