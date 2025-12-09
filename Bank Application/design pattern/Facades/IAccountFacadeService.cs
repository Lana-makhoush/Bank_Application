using Bank_Application.DTOs;

namespace Bank_Application.Services.Facade
{
    public interface IAccountFacadeService
    {
        Task<object> CreateAccountForClient(int clientId, int accountTypeId, int accountStatusId, AccountDto dto);
    }
}
