using Bank_Application.DTOs;
using Bank_Application.Models;
using System.Threading.Tasks;

namespace Bank_Application.Facade
{
    public interface IClientFacade
    {
        Task<Client> RegisterClient(ClientDto dto);
    }
}
