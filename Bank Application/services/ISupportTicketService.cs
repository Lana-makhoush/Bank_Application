using System.Threading.Tasks;

namespace Bank_Application.Services
{
    public interface ISupportTicketService
    {
        Task CreateTicketAsync(int clientId, string subject, string description);
    }
}
