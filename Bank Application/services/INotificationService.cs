namespace Bank_Application.services
{
    public interface INotificationService
    {
        Task NotifyAsync(string eventName, int transactionId);
    }
}