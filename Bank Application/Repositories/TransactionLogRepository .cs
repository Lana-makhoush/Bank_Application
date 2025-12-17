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
    }

    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly AppDbContext _context;

        public RecommendationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Recommendation recommendation)
        {
            _context.Recommendations.Add(recommendation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int clientId, string message)
        {
            return await _context.Recommendations
                .AnyAsync(r => r.ClientId == clientId && r.Message == message);
        }

        public async Task<List<Recommendation>> GetByClientIdAsync(int clientId)
        {
            return await _context.Recommendations
                .Where(r => r.ClientId == clientId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
