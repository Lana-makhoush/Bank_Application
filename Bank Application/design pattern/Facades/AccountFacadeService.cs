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

        public async Task<object> CreateAccountForClient(int clientId, int accountTypeId, int accountStatusId, AccountDto dto)
        {
            if (!await _service.ClientExists(clientId))
                throw new Exception("العميل غير موجود");

            if (!await _service.AccountTypeExists(accountTypeId))
                throw new Exception("نوع الحساب غير موجود");

            if (await _service.ClientHasAccountType(clientId, accountTypeId))
                throw new Exception("لا يمكن إنشاء نفس نوع الحساب لنفس العميل");

            var account = await _service.CreateAccount(accountTypeId, accountStatusId);

            var clientAccount = await _service.CreateClientAccount(clientId, account.AccountId!.Value, dto);

            return clientAccount;
        }


    }
}
