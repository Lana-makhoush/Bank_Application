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
    }

}
