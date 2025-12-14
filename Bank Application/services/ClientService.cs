using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Bank_Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;

        public ClientService(IClientRepository repo)
        {
            _repo = repo;
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public async Task<Client> CreateClient(ClientDto dto)
        {
            if (!decimal.TryParse(dto.MonthlyIncome, out decimal incomeValue) || incomeValue <= 0)
            {
                return null;
            }

            var client = new Client
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                IdentityNumber = dto.IdentityNumber,
                IncomeSource = dto.IncomeSource,

                MonthlyIncome = incomeValue,

                AccountPurpose = dto.AccountPurpose,
                Phone = dto.Phone,
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Email = dto.Email,

            };

            if (dto.IdentityImage != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.IdentityImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.IdentityImage.CopyToAsync(stream);

                client.IdentityImagePath = filePath;
            }

            return await _repo.AddClient(client);
        }


    }
}
