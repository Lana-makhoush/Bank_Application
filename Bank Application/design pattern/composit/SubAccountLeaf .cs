using Bank_Application.DTOs;
using Bank_Application.Patterns.Composite;

public class SubAccountLeaf : IAccountComponent
{
    private readonly SubAccountResponseDto _dto;

    public SubAccountLeaf(SubAccountResponseDto dto)
    {
        _dto = dto;
    }

    public int? AccountId => _dto.SubAccountId;
    public string? AccountName => $"SubAccount-{_dto.SubAccountId}";

    public void AddChild(IAccountComponent child)
    {
        throw new NotSupportedException("SubAccount cannot have children");
    }

    public void RemoveChild(IAccountComponent child)
    {
        throw new NotSupportedException("SubAccount cannot have children");
    }

    public IEnumerable<IAccountComponent> GetChildren()
    {
        return new List<IAccountComponent>();
    }
}
