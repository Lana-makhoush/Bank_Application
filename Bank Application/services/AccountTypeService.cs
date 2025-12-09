using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Factories;

namespace Bank_Application.Services
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly IAccountTypeRepository _repository;
        private readonly factoryAccountType _factory;

        public AccountTypeService(IAccountTypeRepository repository, factoryAccountType factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public AccountType Add(AccountTypeDto dto) 
        {
            var accountType = _factory.CreateAccountType(dto);
            return _repository.Add(accountType);
        }
        public AccountType? Edit(int id, EditAccountTypeDto dto)
        {
            var existing = _repository.GetById(id);
            if (existing == null) return null;

            _factory.UpdateAccountType(existing, dto);
            _repository.Update(existing);

            return existing;
        }
        public bool Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null) return false;

            return _repository.Delete(existing);
        }

    }
}
  