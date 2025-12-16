using Bank_Application.DTOs;
using Bank_Application.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bank_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientFacade _facade;

        public ClientController(IClientFacade facade)
        {
            _facade = facade;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddClient([FromForm] ClientDto dto)
        {
            var client = await _facade.RegisterClient(dto);

            if (client == null)
                return BadRequest(new { message = "فشل في إضافة العميل" });

            var clientData = new
            {
                client.ClientId,
                client.FirstName,
                client.MiddleName,
                client.LastName,
                client.IdentityNumber,
                IdentityImagePath = client.IdentityImagePath,
                client.IncomeSource,
                client.MonthlyIncome,
                client.AccountPurpose,
                client.Phone,
                client.Username,
                client.Email
            };

            return Ok(new { message = "تم إضافة العميل بنجاح", client = clientData });
        }




}
}
