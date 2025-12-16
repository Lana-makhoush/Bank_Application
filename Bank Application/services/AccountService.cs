using Bank_Application.Dtos;
using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;

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
    }
}

