using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Services;
using System.Threading.Tasks;

namespace Bank_Application.Facade
{
    public class ClientFacade : IClientFacade
    {
        private readonly IClientService _service;

        public ClientFacade(IClientService service)
        {
            _service = service;
        }

        public async Task<Client> RegisterClient(ClientDto dto)
        {
            return await _service.CreateClient(dto);
        }
    }
}
