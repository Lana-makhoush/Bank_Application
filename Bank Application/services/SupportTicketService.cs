using Bank_Application.Models;
using Bank_Application.Repositories;
using System.Threading.Tasks;

namespace Bank_Application.Services
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly ISupportTicketRepository _repository;

        public SupportTicketService(ISupportTicketRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateTicketAsync(int clientId, string subject, string description)
        {
            var ticket = new SupportTicket
            {
                ClientId = clientId,
                Subject = subject,
                Description = description,
            };

            await _repository.AddAsync(ticket);
        }
    }
}