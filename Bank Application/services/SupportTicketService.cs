using Bank_Application.Hubs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Bank_Application.Services
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly ISupportTicketRepository _repository;
        private readonly IHubContext<NotificationHub> _hub; 

        public SupportTicketService(ISupportTicketRepository repository, IHubContext<NotificationHub> hub)
        {
            _repository = repository;
            _hub = hub;
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

            await _hub.Clients.Group($"Client_{clientId}")
                .SendAsync("ReceiveReply", "تم إنشاء التذكرة بنجاح! يمكنك انتظار الرد من الموظف.");
        }

        public async Task ReplyToTicketAsync(SupportTicket ticket, string replyText)
        {
            await _hub.Clients.Group($"Client_{ticket.ClientId}")
                .SendAsync("ReceiveReply", replyText);

            
        }
    }
}
