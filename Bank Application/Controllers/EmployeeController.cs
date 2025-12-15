
using Bank_Application.DTOs;
using Bank_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank_Application.Controllers
{
    [ApiController]
    [Route("api/employees")]
    
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromForm] CreateEmployeeDto dto)
        {
            var result = await _service.CreateEmployeeAsync(dto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
     
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginEmployeeDto dto)
        {
            var result = await _service.LoginEmployeeAsync(dto);

            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(result);
        }

        [Authorize(Roles = "Manager,Teller")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto dto)
        {
            var employeeId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _service.ChangePasswordAsync(employeeId, dto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

    }
}

