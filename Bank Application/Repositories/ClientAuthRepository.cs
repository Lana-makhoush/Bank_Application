
using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;
using Bank_Application.Repositories;
namespace Bank_Application.Repositories
{
    public class ClientAuthRepository : IClientAuthRepository
    {
        private readonly AppDbContext _db;

        public ClientAuthRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Client client)
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();
        }

        public async Task<Client?> GetByEmailAsync(string email)
            => await _db.Clients.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<Client?> GetByUsernameAsync(string username)
       => await _db.Clients.FirstOrDefaultAsync(x => x.Username == username);

        public async Task UpdateAsync(Client client)
        {
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();
        }
    }
}