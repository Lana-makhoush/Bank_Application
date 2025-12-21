using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace Bank_Application.Hubs
{

    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var clientId = httpContext?.Request.Query["clientId"].ToString();

            if (!string.IsNullOrEmpty(clientId))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"Client_{clientId}"
                );
            }

            await base.OnConnectedAsync();
        }
    }
}

