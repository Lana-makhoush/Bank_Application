using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Patterns.Composite;
using Bank_Application.Repositories;

namespace Bank_Application.Services
{
    public class AccountHierarchyService : IAccountHierarchyService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ISubAccountService _subAccountService;

        public AccountHierarchyService(IAccountRepository accountRepo, ISubAccountService subAccountService)
        {
            _accountRepo = accountRepo;
            _subAccountService = subAccountService;
        }



        public async Task<AccountComposite> BuildAccountCompositeAsync(int accountId)
        {
            var account = await _accountRepo.GetAccountByIdAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            var composite = new AccountComposite(account.AccountId, "حساب رئيسي");

            var subAccounts = await _subAccountService.GetSubAccountsByParentAsync(accountId);

            var childrenDtos = subAccounts.Select(s => new SubAccountResponseDto
            {
                SubAccountId = (int)s.SubAccountId,
                ParentAccountId = (int)s.ParentAccountId,
                DailyWithdrawalLimit = s.DailyWithdrawalLimit,
                TransferLimit = s.TransferLimit,
                UsageAreas = s.UsageAreas,
                UserPermissions = s.UserPermissions
            }).ToList();

            composite.LoadChildren(childrenDtos);
            return composite;
        }





        public async Task<SubAccountResponseDto> AddSubAccountToAccountAsync(
      SubAccountCreateDto dto,
      int statusId,
      int subAccountTypeId)
        {
            var created = await _subAccountService.CreateSubAccountAsync(
                dto,
                statusId,
                subAccountTypeId
            );

            return new SubAccountResponseDto
            {
                SubAccountId = (int)created.SubAccountId,
                ParentAccountId = (int)created.ParentAccountId,
                DailyWithdrawalLimit = created.DailyWithdrawalLimit,
                TransferLimit = created.TransferLimit,
                UsageAreas = created.UsageAreas,
                UserPermissions = created.UserPermissions,
                Balance = created.Balance,
                CreatedAt = created.CreatedAt,
                StatusName = created.SubAccountStatus?.StatusName
            };
        }



        public async Task<SubAccountResponseDto?> UpdateSubAccountOnAccountAsync(
       int subAccountId,
       int statusId,
       SubAccountUpdateDto dto,
       int subAccountTypeId) // أضف المعامل الجديد
        {
            var updated = await _subAccountService.UpdateSubAccountAsync(subAccountId, statusId, dto, subAccountTypeId);

            if (updated == null) return null;

            return new SubAccountResponseDto
            {
                SubAccountId = (int)updated.SubAccountId,
                ParentAccountId = (int)updated.ParentAccountId,
                DailyWithdrawalLimit = updated.DailyWithdrawalLimit,
                TransferLimit = updated.TransferLimit,
                UsageAreas = updated.UsageAreas,
                UserPermissions = updated.UserPermissions,
                Balance = updated.Balance,
                CreatedAt = updated.CreatedAt,
            };
        }


        public async Task<bool> RemoveSubAccountFromAccountAsync(int subAccountId)
        {
            return await _subAccountService.DeleteSubAccountAsync(subAccountId);
        }
        public async Task<SubAccountResponseDto?> UpdateSubAccountOnAccountAsync(int subAccountId, SubAccountUpdateDto dto, int subAccountTypeId)
        {
            int statusId = 1;

            var updated = await _subAccountService.UpdateSubAccountAsync(subAccountId, statusId, dto, subAccountTypeId);

            if (updated == null) return null;

            return new SubAccountResponseDto
            {
                SubAccountId = (int)updated.SubAccountId,
                ParentAccountId = (int)updated.ParentAccountId,
                DailyWithdrawalLimit = updated.DailyWithdrawalLimit,
                TransferLimit = updated.TransferLimit,
                UsageAreas = updated.UsageAreas,
                UserPermissions = updated.UserPermissions,
                Balance = updated.Balance,
                CreatedAt = updated.CreatedAt,
            };
        }

        public async Task<SubAccountResponseDto?> GetSubAccountByIdAsync(int subAccountId)
{
    var sub = await _subAccountService.GetSubAccountByIdAsync(subAccountId);
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
        public async Task<AccountHierarchyDto?> GetAccountHierarchyAsync(int accountId)
        {
            var composite = await BuildAccountCompositeAsync(accountId);

            var subAccounts = await _subAccountService.GetSubAccountsByParentAsync(accountId);

            var subAccountsDto = subAccounts.Select(s => new SubAccountResponseDto
            {
                SubAccountId = (int)s.SubAccountId,
                ParentAccountId = (int)s.ParentAccountId,
                DailyWithdrawalLimit = s.DailyWithdrawalLimit,
                TransferLimit = s.TransferLimit,
                UsageAreas = s.UsageAreas,
                UserPermissions = s.UserPermissions,
                Balance = s.Balance,
                CreatedAt = s.CreatedAt ?? DateTime.MinValue,
                StatusName = s.SubAccountStatus?.StatusName ?? "غير معروف"
            }).ToList();

            var dto = new AccountHierarchyDto
            {
                AccountId = composite.AccountId,
                AccountName = "حساب رئيسي",  
                SubAccounts = subAccountsDto
            };

            return dto;
        }
        //public async Task<IAccountComponent?> GetHierarchyForClientAsync(int clientId)
        //{
        //    var accounts = await _accountRepo
        //        .GetAccountsWithSubAccountsByClientIdAsync(clientId);

        //    if (accounts == null || !accounts.Any())
        //        return null;

        //    var root = new AccountComposite(null, "حسابات العميل");

        //    foreach (var account in accounts)
        //    {
        //        var accountNode = new AccountComposite(
        //            account.AccountId,
        //            account.AccountType?.TypeName
        //        );

        //        foreach (var sub in account.SubAccounts)
        //        {
        //            var dto = new SubAccountResponseDto
        //            {
        //                SubAccountId = sub.SubAccountId,
        //                ParentAccountId = sub.ParentAccountId ?? 0,
        //                Balance = sub.Balance,
        //                DailyWithdrawalLimit = sub.DailyWithdrawalLimit,
        //                TransferLimit = sub.TransferLimit,
        //                UsageAreas = sub.UsageAreas,
        //                UserPermissions = sub.UserPermissions,
        //                CreatedAt = sub.CreatedAt,
        //                StatusName = sub.SubAccountStatus?.StatusName
        //            };

        //            accountNode.AddChild(new SubAccountLeaf(dto));
        //        }

        //        root.AddChild(accountNode);
        //    }

        //    return root;
        //}


    }
}
