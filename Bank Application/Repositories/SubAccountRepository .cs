using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;

public class SubAccountRepository : ISubAccountRepository
{
    private readonly AppDbContext _context;
    public SubAccountRepository(AppDbContext context) => _context = context;

    public async Task<SubAccount> CreateAsync(SubAccount subAccount)
    {
        _context.SubAccounts.Add(subAccount);
        await _context.SaveChangesAsync();
        return subAccount;
    }

    public async Task<SubAccount?> UpdateAsync(SubAccount subAccount)
    {
        var existing = await _context.SubAccounts.FindAsync(subAccount.SubAccountId);
        if (existing == null) return null;

        existing.DailyWithdrawalLimit = subAccount.DailyWithdrawalLimit;
        existing.TransferLimit = subAccount.TransferLimit;
        existing.UsageAreas = subAccount.UsageAreas;
        existing.UserPermissions = subAccount.UserPermissions;
        existing.SubAccountStatusId = subAccount.SubAccountStatusId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int subAccountId)
    {
        var existing = await _context.SubAccounts.FindAsync(subAccountId);
        if (existing == null) return false;

        _context.SubAccounts.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<SubAccount?> GetByIdAsync(int subAccountId)
    {
        return await _context.SubAccounts
            .Include(s => s.ParentAccount)
            .FirstOrDefaultAsync(s => s.SubAccountId == subAccountId);
    }

    public async Task<bool> ParentAccountExistsAsync(int parentAccountId)
    {
        return await _context.Accounts.AnyAsync(a => a.AccountId == parentAccountId);
    }
    public async Task<bool> ExistsAsync(int parentAccountId, decimal? dailyLimit, decimal? transferLimit, string usageAreas, string userPermissions)
    {
        return await _context.SubAccounts.AnyAsync(s =>
            s.ParentAccountId == parentAccountId &&
            s.DailyWithdrawalLimit == dailyLimit &&
            s.TransferLimit == transferLimit &&
            s.UsageAreas == usageAreas &&
            s.UserPermissions == userPermissions
        );

    }
    public async Task LoadSubAccountStatusAsync(SubAccount subAccount)
    {
        if (subAccount?.SubAccountStatusId != null)
        {
            subAccount.SubAccountStatus = await _context.AccountStatuses
                .FirstOrDefaultAsync(s => s.AccountStatusId == subAccount.SubAccountStatusId.Value);
        }
    }

    public async Task<List<SubAccount>> GetAllAsync()
    {
        return await _context.SubAccounts
            .Include(s => s.SubAccountStatus)
            .ToListAsync();
    }

    public async Task<List<SubAccount>> GetByParentAccountIdAsync(int parentAccountId)
    {
        return await _context.SubAccounts
            .Include(s => s.SubAccountStatus)
            .Where(s => s.ParentAccountId == parentAccountId)
            .ToListAsync();
    }




}
