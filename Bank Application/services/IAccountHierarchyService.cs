using Bank_Application.DTOs;
using Bank_Application.Patterns.Composite;

public interface IAccountHierarchyService
{
    Task<AccountHierarchyDto?> GetAccountHierarchyAsync(int accountId);
    Task<AccountComposite> BuildAccountCompositeAsync(int accountId);
    Task<SubAccountResponseDto> AddSubAccountToAccountAsync(SubAccountCreateDto dto, int statusId);
    Task<SubAccountResponseDto?> UpdateSubAccountOnAccountAsync(int subAccountId, int statusId, SubAccountUpdateDto dto);
    Task<bool> RemoveSubAccountFromAccountAsync(int subAccountId);
    Task<SubAccountResponseDto?> GetSubAccountByIdAsync(int subAccountId);

}
