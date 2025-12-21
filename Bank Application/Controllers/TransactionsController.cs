using Bank_Application.DTOs;
using Bank_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionsController(ITransactionService service)
    {
        _service = service;
    }
    [Authorize(Roles = "Teller")]
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromForm] DepositDto dto)
    {
        var result= await _service.DepositAsync(dto);
        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
    [Authorize(Roles = "Teller")]
    [HttpPost("withdrawal")]
    public async Task<IActionResult> Withdrawal([FromForm] DepositDto dto)
    {
        var result = await _service.WithdrawalAsync(dto);
        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
    [Authorize(Roles = "Teller,User")]
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromForm] TransferDto dto)
    {
        var result = await _service.TransferAsync(dto);
        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

  

}
