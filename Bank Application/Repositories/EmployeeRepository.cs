using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;
namespace Bank_Application.Repositories
{


    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _db;

        public EmployeeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
        }

        public async Task<Employee?> GetByEmailEmployeeAsync(string email)
            => await _db.Employees.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.Employees.AnyAsync(e => e.Email == email);
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _db.Employees.AnyAsync(e => e.Username == username);
        }

        public async Task<Employee?> GetByUsernameAsync(string username)
        {
            return await _db.Employees
                .FirstOrDefaultAsync(e => e.Username == username);
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _db.Employees.FindAsync(id);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();
        }

    }
}
