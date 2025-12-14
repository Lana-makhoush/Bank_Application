using Bank_Application.DTOs;
using Bank_Application.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Bank_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] ClientRegisterDto dto)
        {
            var (success, msg) = await _auth.RegisterClientAsync(dto);
            if (!success) return BadRequest(new { message = msg });
            return Ok(new { message = msg });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromForm] VerifyOtpDto dto)
        {
            var (success, msg) = await _auth.VerifyOtpAsync(dto);
            if (!success) return BadRequest(new { message = msg });
            return Ok(new { message = msg });
        }
        [EnableRateLimiting("login")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            var (success, auth, msg) = await _auth.LoginAsync(dto);
            if (!success) return BadRequest(new { message = msg });
            return Ok(auth);
        }
    }
}
