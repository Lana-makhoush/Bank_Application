using Bank_Application.Models;

namespace Bank_Application.Repositories
{
    public interface IClientAuthRepository
    {
        Task<Client?> GetByEmailAsync(string email);
        Task<Client?> GetByUsernameAsync(string username);
        Task AddAsync(Client client);
        Task UpdateAsync(Client client);
    }
}
