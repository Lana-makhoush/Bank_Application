using Bank_Application.Data;
using Bank_Application.Models;
using Bank_Application.Models;
using System.Collections.Generic; 

namespace Bank_Application.Repositories
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly AppDbContext _context;

        public AccountTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public AccountType Add(AccountType accountType)
        {
            _context.AccountTypes.Add(accountType);
            _context.SaveChanges();
            return accountType;
        }
        public void Update(AccountType accountType)
        {
            _context.AccountTypes.Update(accountType);
            _context.SaveChanges();
        }
        public AccountType? GetById(int id)
        {
            return _context.AccountTypes.Find(id);
        }
        public bool Delete(AccountType accountType)
        {
            if (accountType == null) return false;
            _context.AccountTypes.Remove(accountType);
            _context.SaveChanges();
            return true;
        }
       
        public IEnumerable<AccountType> GetAll()
        {
            return _context.AccountTypes.ToList();
        }
    }
}
