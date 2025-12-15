using Bank_Application.Models;

namespace Bank_Application.Repositories
{

    public interface IEmployeeRepository
    {
        Task<Employee?> GetByEmailEmployeeAsync(string email);
        Task AddAsync(Employee employee);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<Employee?> GetByUsernameAsync(string username);
        Task<Employee?> GetByIdAsync(int id);
        Task UpdateAsync(Employee employee);

    }
}