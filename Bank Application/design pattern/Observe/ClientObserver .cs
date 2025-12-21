
using Bank_Application.Hubs;
using Microsoft.AspNetCore.SignalR;

public class ClientObserver : IClientObserver
{
    private readonly IHubContext<NotificationHub> _hub;
    private readonly int _clientId;

    public ClientObserver(IHubContext<NotificationHub> hub, int clientId)
    {
        _hub = hub;
        _clientId = clientId;
    }

    public async Task UpdateAsync(string message)
    {
        await _hub.Clients.Group($"Client_{_clientId}")
            .SendAsync("ReceiveReply", message);
    }
}