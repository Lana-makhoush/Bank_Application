using Bank_Application.DTOs;
using System.Collections.Generic;

namespace Bank_Application.Patterns.Composite
{
    public class AccountComposite : IAccountComponent
    {
        public int? AccountId { get; private set; }
        public string? AccountName { get; private set; }

        private readonly List<IAccountComponent> _children = new();

        public AccountComposite(int? accountId, string? accountName)
        {
            AccountId = accountId;
            AccountName = accountName;
        }

        public void AddChild(IAccountComponent child)
        {
            _children.Add(child);
        }

        public void RemoveChild(IAccountComponent child)
        {
            _children.Remove(child);
        }

        public IEnumerable<IAccountComponent> GetChildren()
        {
            return _children;
        }

        public void LoadChildren(IEnumerable<object> childrenDtos)
        {
            foreach (var dto in childrenDtos)
            {
                var subAccount = dto as SubAccountResponseDto;
                if (subAccount != null)
                {
                    AddChild(new SubAccountLeaf(subAccount));
                }
            }
        }
    }
}