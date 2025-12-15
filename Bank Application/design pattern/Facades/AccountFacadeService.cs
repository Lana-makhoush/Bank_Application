using Bank_Application.DTOs;
using Bank_Application.Models;

namespace Bank_Application.Services.Facade
{
    public class AccountFacadeService : IAccountFacadeService
    {
        private readonly IAccountService _service;

        public AccountFacadeService(IAccountService service)
        {
            _service = service;
        }

        public async Task<object?> CreateAccountForClient(
     int clientId,
     int accountTypeId,
     int accountStatusId,
     AccountDto dto)
        {
            if (!await _service.ClientExists(clientId))
                return null;

            if (!await _service.AccountTypeExists(accountTypeId))
                return "ACCOUNT_TYPE_NOT_FOUND";

            if (await _service.ClientHasAccountType(clientId, accountTypeId))
                return "DUPLICATE_ACCOUNT_TYPE";

            var account = await _service.CreateAccount(accountTypeId, accountStatusId);

            var clientAccount =
                await _service.CreateClientAccount(clientId, account.AccountId!.Value, dto);

            return clientAccount;
        }


    }
}