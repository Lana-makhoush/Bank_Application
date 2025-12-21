public interface IClientObserver
{
    Task UpdateAsync(string message);
}