using Bank_Application.Dtos;
using Bank_Application.DTOs;
using Bank_Application.Models;

namespace Bank_Application.Services
{
    public interface IAccountService
    {
        Task<bool> ClientExists(int clientId);
        Task<bool> AccountTypeExists(int accountTypeId);
        Task<Account> CreateAccount(int accountTypeId, int accountStatusId);

        Task<ClientAccount> CreateClientAccount(int clientId, int accountId, AccountDto dto);
        Task<Client?> GetClientById(int clientId);
        Task<AccountType?> GetAccountTypeById(int accountTypeId);
        Task<bool> ClientHasAccountType(int clientId, int accountTypeId);
        Task<ClientAccount?> UpdateAccount(int clientAccountId, int newAccountTypeId, UpdateAccountDto dto);


        Task<ClientAccount?> GetClientAccountById(int clientAccountId);
        Task<List<ClientAccount>> GetAllClientAccounts();
        Task<List<Feature>> GetFeaturesByAccountType(int accountTypeId);
        Task<List<SubAccount>> GetAllSubAccounts();
        Task<List<object>> DeductFeaturesFromAccount(ClientAccount clientAccount, List<Feature> features);
        Task<List<object>> DeductFeaturesFromSubAccount(SubAccount subAccount, List<Feature> features);

        Task DeductAllFeaturesFromAccounts();
        Task UpdateClientAccountBalance(int clientAccountId, decimal newBalance);
        Task UpdateSubAccountBalance(int subAccountId, decimal newBalance);

    }
}
