using Bank_Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bank_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountStateController : ControllerBase
    {
        private readonly IClientAccountRepository _clientRepo;
        private readonly ISubAccountRepository _subRepo;

        public AccountStateController(IClientAccountRepository clientRepo, ISubAccountRepository subRepo)
        {
            _clientRepo = clientRepo;
            _subRepo = subRepo;
        }

        [HttpPost("close/client/{id}")]
        public async Task<IActionResult> CloseClientAccount(int id)
        {
            var result = await _clientRepo.CloseAccountAsync(id);
            if (!result.Success)
                return Conflict(new { status = 409, message = result.Message });

            return Ok(new { status = 200, message = result.Message });
        }

        [HttpPost("activate/client/{id}")]
        public async Task<IActionResult> ActivateClientAccount(int id)
        {
            var result = await _clientRepo.ActivateAccountAsync(id);
            if (!result.Success)
                return Conflict(new { status = 409, message = result.Message });

            return Ok(new { status = 200, message = result.Message });
        }

        [HttpPost("close/sub/{id}")]
        public async Task<IActionResult> CloseSubAccount(int id)
        {
            var result = await _subRepo.CloseSubAccountAsync(id);
            if (!result.Success)
                return Conflict(new { status = 409, message = result.Message });

            return Ok(new { status = 200, message = result.Message });
        }

        [HttpPost("activate/sub/{id}")]
        public async Task<IActionResult> ActivateSubAccount(int id)
        {
            var result = await _subRepo.ActivateSubAccountAsync(id);
            if (!result.Success)
                return Conflict(new { status = 409, message = result.Message });

            return Ok(new { status = 200, message = result.Message });
        }

        [HttpPost("suspend/client/{id}")]
        public async Task<IActionResult> SuspendClientAccount(int id)
        {
            var result = await _clientRepo.SuspendAccountAsync(id);
            if (!result.Success) return Conflict(new { status = 409, message = result.Message });
            return Ok(new { status = 200, message = result.Message });
        }

        [HttpPost("freeze/client/{id}")]
        public async Task<IActionResult> FreezeClientAccount(int id)
        {
            var result = await _clientRepo.FreezeAccountAsync(id);
            if (!result.Success) return Conflict(new { status = 409, message = result.Message });
            return Ok(new { status = 200, message = result.Message });
        }
        [HttpPost("suspend/sub/{id}")]
        public async Task<IActionResult> SuspendSubAccount(int id)
        {
            var result = await _subRepo.SuspendSubAccountAsync(id);
            if (!result.Success)
                return Conflict(new { status = 409, message = result.Message });

            return Ok(new { status = 200, message = result.Message });
        }


        [HttpPost("freeze/sub/{id}")]
        public async Task<IActionResult> FreezeSubAccount(int id)
        {
            var result = await _subRepo.FreezeSubAccountAsync(id);
            if (!result.Success)
                return Conflict(new { status = 409, message = result.Message });

            return Ok(new { status = 200, message = result.Message });



        }

    }
}
