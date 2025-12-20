using Bank_Application.DTOs;
using Bank_Application.Patterns.Composite;
using Bank_Application.Repositories;
using Bank_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubAccountController : ControllerBase
    {
        private readonly ISubAccountService _subAccountService;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountHierarchyService _accountHierarchyService;

        public SubAccountController(ISubAccountService subAccountService, IAccountRepository accountRepository, IAccountHierarchyService accountHierarchyService)
        {
            _subAccountService = subAccountService;
            _accountRepository = accountRepository;
            _accountHierarchyService = accountHierarchyService;
        }


        [Authorize(Roles = "Manager,Teller")]

        [HttpPost("{accountId:int}/{statusId:int}/{subAccountTypeId:int}/Add_Sub_Account")]
        public async Task<IActionResult> Create(
     int accountId,
     int statusId,
     int subAccountTypeId,
     [FromForm] SubAccountCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ParentAccountId = accountId;

            try
            {
                var created = await _subAccountService.CreateSubAccountAsync(
                    dto,
                    statusId,
                    subAccountTypeId
                );

                var response = new SubAccountResponseDto
                {
                    SubAccountId = (int)created.SubAccountId,
                    ParentAccountId = (int)created.ParentAccountId,
                    DailyWithdrawalLimit = created.DailyWithdrawalLimit,
                    TransferLimit = created.TransferLimit,
                    UsageAreas = created.UsageAreas,
                    UserPermissions = created.UserPermissions,
                    Balance = created.Balance,
                    CreatedAt = created.CreatedAt,
                    StatusName = created.SubAccountStatus?.StatusName
                };

                return StatusCode(201, new
                {
                    StatusCode = 201,
                    Message = "تم إنشاء الحساب الفرعي بنجاح",
                    Data = response
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    StatusCode = 409,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "خطأ: " + ex.Message
                });
            }
        }

        [Authorize(Roles = "Manager,Teler")]

        [HttpPut("{subAccountId}/{statusId}/{subAccountTypeId}/Update_Sub_Account")]
        public async Task<IActionResult> Update(
    int subAccountId,
    int statusId,
    int subAccountTypeId,
    [FromForm] SubAccountUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _subAccountService.UpdateSubAccountAsync(
                    subAccountId,
                    statusId,
                    dto,
                    subAccountTypeId
                );

                if (updated == null)
                    return NotFound(new { StatusCode = 404, Message = "الحساب الفرعي غير موجود" });

                var response = new SubAccountResponseDto
                {
                    SubAccountId = updated.SubAccountId,
                    ParentAccountId = updated.ParentAccountId ?? 0,
                    DailyWithdrawalLimit = updated.DailyWithdrawalLimit,
                    TransferLimit = updated.TransferLimit,
                    UsageAreas = updated.UsageAreas,
                    UserPermissions = updated.UserPermissions,
                    Balance = updated.Balance,
                    CreatedAt = updated.CreatedAt,
                    StatusName = updated.SubAccountStatus?.StatusName
                };

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "تم تعديل الحساب الفرعي بنجاح",
                    Data = response
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { StatusCode = 409, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "خطأ: " + ex.Message });
            }
        }


        [Authorize(Roles = "Manager,Teller")]

        [HttpDelete("Delete_sub_Account/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _subAccountService.DeleteSubAccountAsync(id);

            if (!deleted)
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"الحساب الفرعي بالمعرف {id} غير موجود"
                });

            return Ok(new
            {
                StatusCode = 200,
                Message = "تم حذف الحساب الفرعي بنجاح",
                
            });
        }
        [Authorize(Roles = "Manager,Teller")]

        [HttpGet("Get_Sub_Account/{id}")]
        public async Task<IActionResult> GetSubAccountById(int id)
        {
            var sub = await _subAccountService.GetSubAccountByIdResponseAsync(id);

            if (sub == null)
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "الحساب الفرعي غير موجود"
                });

            return Ok(new
            {
                StatusCode = 200,
                Message = "تم جلب بيانات الحساب الفرعي بنجاح",
                Data = sub
            });
        }
        [Authorize(Roles = "Manager,Teller")]

        [HttpGet("Get_All_Sub_Accounts")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _subAccountService.GetAllSubAccountsAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "تم جلب جميع الحسابات الفرعية بنجاح",
                Data = list
            });
        }
        [Authorize(Roles = "Manager,Teller")]
        [HttpGet("{accountId}/hierarchy")]
        public async Task<IActionResult> GetAccountHierarchy(int accountId)
        {
            var accountHierarchy = await _accountHierarchyService.GetAccountHierarchyAsync(accountId);
            if (accountHierarchy == null)
            {
                return NotFound(new { message = "الحساب الرئيسي غير موجود" });
            }

            return Ok(accountHierarchy);
        }
        //
        [Authorize(Roles = "User")]
        [HttpGet("my-accounts/hierarchy")]
        public async Task<IActionResult> GetMyAccountsHierarchy()
        {
            var clientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (clientIdClaim == null)
                return Unauthorized(new { message = "المعرف غير موجود في التوكن" });

            int clientId = int.Parse(clientIdClaim.Value);

            var clientAccounts = await _accountRepository
                .GetAccountsWithSubAccountsByClientIdAsync(clientId);

            if (clientAccounts == null || !clientAccounts.Any())
                return NotFound(new { message = "لا يوجد حسابات مرتبطة بهذا العميل" });

            var result = new List<AccountHierarchyDto>();

            foreach (var account in clientAccounts)
            {
                var hierarchy = await _accountHierarchyService
                    .GetAccountHierarchyAsync(account.AccountId);

                if (hierarchy != null)
                    result.Add(hierarchy);
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "تم جلب التسلسل الهرمي لحسابات العميل بنجاح",
                Data = result
            });
        }



    }
}
