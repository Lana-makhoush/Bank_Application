using Bank_Application.Models;

namespace Bank_Application.Services
{
    public interface IAccountTypeFacade
    {
        bool Delete(int id);
        IEnumerable<AccountType> GetAll();
        AccountType? GetById(int id);
    }
}
