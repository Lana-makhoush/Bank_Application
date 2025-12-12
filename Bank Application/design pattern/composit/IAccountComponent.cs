namespace Bank_Application.Patterns.Composite
{
    public interface IAccountComponent
    {
        int? AccountId { get; }
        string? AccountName { get; }
        void AddChild(IAccountComponent child);
        void RemoveChild(IAccountComponent child);
        IEnumerable<IAccountComponent> GetChildren();
    }
}
