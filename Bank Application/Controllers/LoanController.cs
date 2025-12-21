using Bank_Application.DTOs;
using Bank_Application.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Application.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly LoanService _service;

        public LoanController(LoanService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateLoanDto dto)
        {
            var result = await _service.CreateLoanAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }

}
