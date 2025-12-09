using Bank_Application.Models;
using System.Collections.Generic;

namespace Bank_Application.Repositories
{
    public interface IAccountTypeRepository
    {
        AccountType Add(AccountType accountType);
        AccountType? GetById(int id);
        IEnumerable<AccountType> GetAll();
        void Update(AccountType accountType);
        bool Delete(AccountType accountType);
    }
}
