using Bank_Application.Dtos;
using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;

        public AccountService(IAccountRepository repo)
        {
            _repo = repo;
        }

        public Task<bool> ClientExists(int clientId) => _repo.ClientExists(clientId);

        public Task<bool> AccountTypeExists(int accountTypeId) => _repo.AccountTypeExists(accountTypeId);

        public async Task<Account> CreateAccount(int accountTypeId, int accountStatusId)
        {
            var account = new Account
            {
                AccountTypeId = accountTypeId,
                AccountStatusId = accountStatusId,
                ParentAccountId = null
            };

            return await _repo.CreateAccount(account);
        }


        public async Task<ClientAccount> CreateClientAccount(
     int clientId,
     int accountId,
     AccountDto dto)
        {
            var ca = new ClientAccount
            {
                ClientId = clientId,
                AccountId = accountId,
                Balance = dto.Balance,
                CreatedAt = dto.CreatedAt
            };

            await _repo.CreateClientAccount(ca);

            return await _repo.GetClientAccountById(ca.Id)!;
        }


        public Task<Client?> GetClientById(int clientId) => _repo.GetClientById(clientId);

        public Task<AccountType?> GetAccountTypeById(int accountTypeId) => _repo.GetAccountTypeById(accountTypeId);

        public Task<bool> ClientHasAccountType(int clientId, int accountTypeId)
        {
            return _repo.ClientHasAccountType(clientId, accountTypeId);
        }
        public async Task<ClientAccount?> UpdateAccount(int clientAccountId, int newAccountTypeId, UpdateAccountDto dto)
        {
            return await _repo.UpdateAccount(clientAccountId, newAccountTypeId, dto.Balance!.Value);
        }


        public Task<ClientAccount?> GetClientAccountById(int clientAccountId)
        {
            return _repo.GetClientAccountById(clientAccountId);
        }
        public Task<List<ClientAccount>> GetAllClientAccounts()
            => _repo.GetAllClientAccounts();


        public async Task<List<Feature>> GetFeaturesByAccountType(int accountTypeId)
        {
            var accountType = await _repo.GetAccountTypeById(accountTypeId);
            if (accountType?.AccountTypeFeatures == null)
                return new List<Feature>();

            return accountType.AccountTypeFeatures
                              .Where(atf => atf.Feature != null)
                              .Select(atf => atf.Feature!)
                              .ToList();
        }

        
        public Task UpdateClientAccountBalance(int id, decimal balance)
            => _repo.UpdateClientAccountBalance(id, balance);

        public Task UpdateSubAccountBalance(int id, decimal balance)
            => _repo.UpdateSubAccountBalance(id, balance);
        public async Task<List<SubAccount>> GetAllSubAccounts()
        {
            return await _repo.GetAllSubAccounts();
        }
      
      

        public async Task DeductAllFeaturesFromAccounts()
        {
            // 1️⃣ الحسابات الرئيسية
            var clientAccounts = await _repo.GetAllClientAccounts();

            foreach (var ca in clientAccounts)
            {
                if (ca.Account?.AccountTypeId != null)
                {
                    var features = await GetFeaturesByAccountType(ca.Account.AccountTypeId.Value);
                    await DeductFeaturesFromAccount(ca, features);
                }
            }

            // 2️⃣ الحسابات الفرعية
            var subAccounts = await _repo.GetAllSubAccounts();

            foreach (var sa in subAccounts)
            {
                if (sa.SubAccountTypeId != null)
                {
                    var features = await GetFeaturesByAccountType(sa.SubAccountTypeId.Value);
                    await DeductFeaturesFromSubAccount(sa, features);
                }
            }
        }

        public async Task<List<object>> DeductFeaturesFromAccount(ClientAccount account, List<Feature> features)
        {
            decimal balance = account.Balance ?? 0;

            foreach (var feature in features)
            {
                decimal cost = feature.Cost; 
                balance -= cost;
                if (balance < 0) balance = 0;
            }

            account.Balance = balance;

            await _repo.UpdateClientAccountBalance(account.Id, balance);

            return new List<object>();
        }

        public async Task<List<object>> DeductFeaturesFromSubAccount(SubAccount account, List<Feature> features)
        {
            decimal balance = account.Balance ?? 0;

            foreach (var feature in features)
            {
                decimal cost = feature.Cost; 
                balance -= cost;
                if (balance < 0) balance = 0;
            }

            account.Balance = balance;

            await _repo.UpdateSubAccountBalance(account.SubAccountId, balance);

            return new List<object>();
        }






    }
}



