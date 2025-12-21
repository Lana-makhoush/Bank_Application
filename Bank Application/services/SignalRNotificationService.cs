//using Bank_Application.services;
//using Microsoft.AspNetCore.SignalR;

//public class SignalRNotificationService : INotificationService
//{
//    private readonly IHubContext<NotificationHub> _hub;

//    public SignalRNotificationService(IHubContext<NotificationHub> hub)
//    {
//        _hub = hub;
//    }

//    public async Task NotifyAsync(string eventName, int transactionId)
//    {
//        await _hub.Clients.All.SendAsync(eventName, transactionId);
//    }
//}
