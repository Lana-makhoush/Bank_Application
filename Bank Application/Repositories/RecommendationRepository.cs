using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.Repositories
{
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

        public async Task<List<object>> GetByClientIdAsync(int clientId)
        {
            return await _context.Recommendations
                .Where(r => r.ClientId == clientId)
                .Include(r => r.Client)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    clientId = r.ClientId,
                    clientFullName =
                        r.Client.FirstName + " " +
                        r.Client.MiddleName + " " +
                        r.Client.LastName,
                    message = r.Message,
                    createdDate = r.CreatedAt.Date
                })
                .Cast<object>()
                .ToListAsync();
        }
    }
}

