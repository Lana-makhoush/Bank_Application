using Bank_Application.Models;
using Bank_Application.Repositories;

namespace Bank_Application.Services
{
    public class AccountTypeFacade : IAccountTypeFacade
    {
        private readonly IAccountTypeRepository _repository;

        public AccountTypeFacade(IAccountTypeRepository repository)
        {
            _repository = repository;
        }

        public bool Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null) return false;

            return _repository.Delete(existing);
        }
        public IEnumerable<AccountType> GetAll()
        {
            return _repository.GetAll();
        }

        public AccountType? GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
