using Bank_Application.DTOs;
using Bank_Application.Models;

namespace Bank_Application.Services
{
    public interface IAccountTypeService
    {
        AccountType Add(AccountTypeDto dto);
        AccountType? Edit(int id, EditAccountTypeDto dto);
          bool Delete(int id);
    }
}
