using Bank_Application.Models;
using System.Threading.Tasks;

namespace Bank_Application.Repositories
{
    public interface ISupportTicketRepository
    {
        Task AddAsync(SupportTicket ticket);
    }
}
