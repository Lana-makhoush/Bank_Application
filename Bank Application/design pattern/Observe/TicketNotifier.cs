using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TicketNotifier
{
    private readonly List<IClientObserver> _observers = new List<IClientObserver>();

    public void Attach(IClientObserver observer) => _observers.Add(observer);
    public void Detach(IClientObserver observer) => _observers.Remove(observer);

    public async Task NotifyAllAsync(string message)
    {
        foreach (var observer in _observers)
        {
            await observer.UpdateAsync(message);
        }
    }
}