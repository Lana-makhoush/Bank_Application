using Bank_Application.Models;

namespace Bank_Application.Repositories
{

    public interface IEmployeeRepository
    {
        Task<Employee?> GetByEmailEmployeeAsync(string email);
        Task AddAsync(Employee employee);
    }
}