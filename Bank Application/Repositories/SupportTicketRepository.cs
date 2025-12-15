using Bank_Application.Models;
using Bank_Application.Data;
using System.Threading.Tasks;

namespace Bank_Application.Repositories
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        private readonly AppDbContext _context;

        public SupportTicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SupportTicket ticket)
        {
            _context.SupportTickets.Add(ticket);
            await _context.SaveChangesAsync();
        }
    }
}