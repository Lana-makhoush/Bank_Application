using Bank_Application.DTOs;
using Bank_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bank_Application.Controllers
{
    [ApiController]
    [Route("api/support-tickets")]
    [Authorize(Roles = "User")]
    public class SupportTicketController : ControllerBase
    {
        private readonly ISupportTicketService _service;

        public SupportTicketController(ISupportTicketService service)
        {
            _service = service;
        }

        [HttpPost]

        public async Task<IActionResult> CreateTicket([FromForm] CreateSupportTicketDto dto)
        {
            var clientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _service.CreateTicketAsync(clientId, dto.Subject, dto.Description);

            return Ok(new { message = "تم إنشاء تذكرة الدعم بنجاح" });
        }
    }
}