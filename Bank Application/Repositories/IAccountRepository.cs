using Bank_Application.Models;

namespace Bank_Application.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> CreateAccount(Account account);
        Task<ClientAccount> CreateClientAccount(ClientAccount clientAccount);
        Task<bool> ClientExists(int clientId);
        Task<bool> AccountTypeExists(int accountTypeId);
        Task<Client?> GetClientById(int clientId);
        Task<AccountType?> GetAccountTypeById(int accountTypeId);

        Task<bool> ClientHasAccountType(int clientId, int accountTypeId);
        Task<ClientAccount?> UpdateAccount(int clientAccountId, int newAccountTypeId, decimal newBalance);

        Task<ClientAccount?> GetClientAccountById(int clientAccountId);
        Task<List<ClientAccount>> GetAllClientAccounts();
        Task<Account?> GetAccountByIdAsync(int accountId);
        Task<List<Feature>> GetFeaturesByAccountType(int accountTypeId);
        Task<List<SubAccount>> GetAllSubAccounts();
        //Task UpdateSubAccountBalance(int subAccountId, decimal newBalance);
        Task UpdateClientAccountBalance(int clientAccountId, decimal newBalance);
        Task UpdateSubAccountBalance(int subAccountId, decimal newBalance);

        Task<List<Account>> GetAccountsWithSubAccountsByClientIdAsync(int clientId);


    }
}

