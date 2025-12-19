using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Patterns.Composite;

public interface ISubAccountService
{
    Task<SubAccount> CreateSubAccountAsync(
     SubAccountCreateDto dto,
     int statusId,
     int subAccountTypeId
 );

    Task<SubAccount?> UpdateSubAccountAsync(
         int subAccountId,
         int statusId,
         SubAccountUpdateDto dto,
         int subAccountTypeId
     );
    Task<bool> DeleteSubAccountAsync(int subAccountId);
    Task<List<SubAccount>> GetSubAccountsByParentAsync(int parentAccountId);
    Task<SubAccount?> GetSubAccountByIdAsync(int subAccountId);
    Task<List<SubAccountResponseDto>> GetAllSubAccountsAsync();
    Task<SubAccountResponseDto?> GetSubAccountByIdResponseAsync(int subAccountId);
//

}
