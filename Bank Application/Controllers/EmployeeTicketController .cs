using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

[ApiController]
[Route("api/employee-tickets")]
public class EmployeeTicketController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hub;
    private readonly TicketNotifier _notifier;

    public EmployeeTicketController(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
        _notifier = new TicketNotifier();
    }
    [Authorize(Roles = "Manager,Teller")]

    [HttpPost("reply")]
    public async Task<IActionResult> SendReply(
        [FromQuery] int clientId,
        [FromForm] string replyText
    )
    {
        await _hub.Clients
            .Group($"Client_{clientId}")
            .SendAsync("ReceiveReply", replyText);

        return Ok(new { message = "تم إرسال الرد" });
    }

}
