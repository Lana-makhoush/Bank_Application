using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Patterns.Composite;
using Bank_Application.Repositories;
using Bank_Application.Patterns.Composite;

public class SubAccountService : ISubAccountService
{
    private readonly ISubAccountRepository _repo;

    public SubAccountService(ISubAccountRepository repo)
    {
        _repo = repo;
    }

    public async Task<SubAccount> CreateSubAccountAsync(SubAccountCreateDto dto, int statusId)
    {
        var parentExists = await _repo.ParentAccountExistsAsync(dto.ParentAccountId);
        if (!parentExists)
            throw new InvalidOperationException("الحساب الرئيسي غير موجود");

        if (dto.DailyWithdrawalLimit == null)
            throw new InvalidOperationException("حد السحب اليومي مطلوب");

        if (dto.TransferLimit == null)
            throw new InvalidOperationException("حد التحويل مطلوب");

        var sub = new SubAccount
        {
            ParentAccountId = dto.ParentAccountId,
            DailyWithdrawalLimit = dto.DailyWithdrawalLimit.Value,
            TransferLimit = dto.TransferLimit.Value,
            UsageAreas = dto.UsageAreas,
            UserPermissions = dto.UserPermissions,
            Balance = dto.Balance,
            CreatedAt = dto.CreatedAt,
            SubAccountStatusId = statusId
        };

        var created = await _repo.CreateAsync(sub);

        await _repo.LoadSubAccountStatusAsync(created);

        return created;
    }

    public async Task<SubAccount?> UpdateSubAccountAsync(int subAccountId, int statusId, SubAccountUpdateDto dto)
    {
        var sub = await _repo.GetByIdAsync(subAccountId);
        if (sub == null)
            return null;

        if (!string.IsNullOrWhiteSpace(dto.DailyWithdrawalLimit))
        {
            if (!decimal.TryParse(dto.DailyWithdrawalLimit, out var daily))
                throw new InvalidOperationException("حد السحب اليومي غير صالح");

            sub.DailyWithdrawalLimit = daily;
        }

        if (!string.IsNullOrWhiteSpace(dto.TransferLimit))
        {
            if (!decimal.TryParse(dto.TransferLimit, out var transfer))
                throw new InvalidOperationException("حد التحويل غير صالح");

            sub.TransferLimit = transfer;
        }

        if (!string.IsNullOrWhiteSpace(dto.UsageAreas))
            sub.UsageAreas = dto.UsageAreas;

        if (!string.IsNullOrWhiteSpace(dto.UserPermissions))
            sub.UserPermissions = dto.UserPermissions;

        if (!string.IsNullOrWhiteSpace(dto.Balance))
        {
            if (!decimal.TryParse(dto.Balance, out var balance))
                throw new InvalidOperationException("الرصيد غير صالح");

            sub.Balance = balance;
        }

        if (dto.CreatedAt.HasValue)
            sub.CreatedAt = dto.CreatedAt.Value;

       
        

        sub.SubAccountStatusId = statusId;

        var updated = await _repo.UpdateAsync(sub);

        await _repo.LoadSubAccountStatusAsync(updated);

        return updated;
    }




    public async Task<bool> DeleteSubAccountAsync(int subAccountId)
    {
        return await _repo.DeleteAsync(subAccountId);
    }

    public async Task<List<SubAccount>> GetSubAccountsByParentAsync(int parentAccountId)
    {
        return await _repo.GetByParentAccountIdAsync(parentAccountId);
    }
    public async Task<SubAccount?> GetSubAccountByIdAsync(int subAccountId)
    {
        var sub = await _repo.GetByIdAsync(subAccountId);
        if (sub == null)
            return null;

        await _repo.LoadSubAccountStatusAsync(sub); 
        return sub;
    }
    public async Task<List<SubAccountResponseDto>> GetAllSubAccountsAsync()
    {
        var list = await _repo.GetAllAsync();

        return list.Select(s => new SubAccountResponseDto
        {
            SubAccountId = s.SubAccountId ?? 0,
            ParentAccountId = s.ParentAccountId ?? 0,
            DailyWithdrawalLimit = s.DailyWithdrawalLimit,
            TransferLimit = s.TransferLimit,
            UsageAreas = s.UsageAreas,
            UserPermissions = s.UserPermissions,
            Balance = s.Balance, 
            CreatedAt = s.CreatedAt ?? DateTime.MinValue,
            StatusName = s.SubAccountStatus?.StatusName ?? "غير معروف" 
        }).ToList();
    }



  
    public async Task<SubAccountResponseDto?> GetSubAccountByIdResponseAsync(int subAccountId)
{
    var sub = await GetSubAccountByIdAsync(subAccountId);
    if (sub == null) return null;

    return new SubAccountResponseDto
    {
        SubAccountId = (int)sub.SubAccountId,
        ParentAccountId = (int)sub.ParentAccountId,
        DailyWithdrawalLimit = sub.DailyWithdrawalLimit,
        TransferLimit = sub.TransferLimit,
        UsageAreas = sub.UsageAreas,
        UserPermissions = sub.UserPermissions,
        Balance = sub.Balance,
        CreatedAt = sub.CreatedAt,
        StatusName = sub.SubAccountStatus?.StatusName
    };
}


}
