using Bank_Application.DTOs;
using Bank_Application.Models;
using System.Threading.Tasks;

namespace Bank_Application.Services
{
    public interface IClientService
    {
        Task<Client> CreateClient(ClientDto dto);
    }
}
