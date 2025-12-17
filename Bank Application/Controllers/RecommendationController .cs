using Bank_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bank_Application.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    [Authorize(Roles = "User")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _service;

        public RecommendationController(IRecommendationService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate()
        {
            var clientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _service.GenerateAsync(clientId);

            return Ok(new { message = "تم توليد التوصيات بنجاح" });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.GetClientRecommendationsAsync(clientId);
            return Ok(result);
        }
    }
}
