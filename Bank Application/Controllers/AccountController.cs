using Bank_Application.Dtos;
using Bank_Application.DTOs;
//using Bank_Application.Migrations;
using Bank_Application.Models;
using Bank_Application.Services;
using Bank_Application.Services.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountFacadeService _facade;

        private readonly IAccountService _service;

        public AccountController(IAccountFacadeService facade, IAccountService service)
        {
            _facade = facade;
            _service = service;
        }

        [Authorize(Roles = "Teller,Manager")]
        [HttpPost("add-account/{clientId:int}/{accountTypeId:int}/1")]
        public async Task<IActionResult> AddAccount(
     int clientId,
     int accountTypeId,
     [FromForm] AccountDto dto)
        {
            var result = await _facade.CreateAccountForClient(clientId, accountTypeId, 1, dto);

            if (result is string errorMessage) 
            {
                return BadRequest(new
                {
                    status = 400,
                    message = errorMessage
                });
            }

            if (result == null) 
            {
                return NotFound(new
                {
                    status = 404,
                    message = "العميل غير موجود"
                });
            }

           
            return Ok(new
            {
                status = 200,
                message = "تم إنشاء الحساب بنجاح"
            });
        }

        [Authorize(Roles = "Teller")]
        [HttpPut("UpdateAccount/{clientAccountId}/{newAccountTypeId}")]
        public async Task<IActionResult> UpdateAccount(
     int clientAccountId,
     int newAccountTypeId,
     [FromForm] UpdateAccountDto dto)
        {
            var updated = await _service.UpdateAccount(clientAccountId, newAccountTypeId, dto);

            if (updated == null)
                return NotFound(new { status = 404, message = "الحساب غير موجود" });

            return Ok(new
            {
                status = 200,
                message = "تم تعديل الحساب بنجاح",
                data = new
                {
                    clientAccountId = updated.Id,
                    accountId = updated.AccountId,
                    clientFullName = $"{updated.Client!.FirstName} {updated.Client.MiddleName} {updated.Client.LastName}",
                    accountTypeName = updated.Account!.AccountType!.TypeName,
                    balance = updated.Balance,
                    createdAt = updated.CreatedAt?.ToString("yyyy-MM-dd")
                }
            });
        }


        [Authorize(Roles = "Teller,Manager")]
        [HttpGet("GetAccountByClientAccountId/{clientAccountId}")]
        public async Task<IActionResult> GetAccountByClientAccountId(int clientAccountId)
        {
            var account = await _service.GetClientAccountById(clientAccountId);

            if (account == null)
                return NotFound(new { status = 404, message = "الحساب غير موجود" });

            return Ok(new
            {
                status = 200,
                data = new
                {
                    clientAccountId = account.Id,     
                    accountId = account.AccountId,     
                    clientFullName = $"{account.Client!.FirstName} {account.Client.MiddleName} {account.Client.LastName}",
                    accountTypeName = account.Account!.AccountType!.TypeName,
                    balance = account.Balance,
                    createdAt = account.CreatedAt?.ToString("yyyy-MM-dd")
                }
            });


        }

        [Authorize(Roles = "Teller,Manager")]
        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _service.GetAllClientAccounts();
            var data = accounts.Select(account => new
            {
                clientAccountId = account.Id,
                accountId = account.AccountId,
                clientFullName = $"{account.Client!.FirstName} {account.Client.MiddleName} {account.Client.LastName}",
                accountTypeName = account.Account!.AccountType!.TypeName,
                balance = account.Balance,
                createdAt = account.CreatedAt?.ToString("yyyy-MM-dd")
            });


            return Ok(new { status = 200, data });
        }

      

    }
}