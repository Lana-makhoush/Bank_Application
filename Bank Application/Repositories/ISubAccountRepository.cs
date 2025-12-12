using Bank_Application.Models;

public interface ISubAccountRepository
{
    Task<SubAccount> CreateAsync(SubAccount subAccount);
    Task<SubAccount?> UpdateAsync(SubAccount subAccount);
    Task<bool> DeleteAsync(int subAccountId);
    Task<List<SubAccount>> GetByParentAccountIdAsync(int parentAccountId);
    Task<SubAccount?> GetByIdAsync(int subAccountId);
    Task<bool> ParentAccountExistsAsync(int parentAccountId);
    Task<bool> ExistsAsync(int parentAccountId, decimal? dailyLimit, decimal? transferLimit, string usageAreas, string userPermissions);
    Task LoadSubAccountStatusAsync(SubAccount subAccount);
    Task<List<SubAccount>> GetAllAsync();

}
