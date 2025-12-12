using Bank_Application.DTOs;
using Bank_Application.Patterns.Composite;
using Bank_Application.Repositories;
using Bank_Application.Services;
using Microsoft.AspNetCore.Mvc;

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

       

        [HttpPost("{accountId}/{statusId}/Add_Sub_Account")]
        public async Task<IActionResult> Create(int accountId, int statusId, [FromForm] SubAccountCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ParentAccountId = accountId;

            try
            {
                var created = await _subAccountService.CreateSubAccountAsync(dto, statusId);

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
                return Conflict(new { StatusCode = 409, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "خطأ: " + ex.Message });
            }
        }

        [HttpPut("{subAccountId}/{statusId}/Update_Sub_Account")]
        public async Task<IActionResult> Update(int subAccountId, int statusId, [FromForm] SubAccountUpdateDto dto)
        {
            try
            {
                var updated = await _subAccountService.UpdateSubAccountAsync(subAccountId, statusId, dto);

                if (updated == null)
                    return NotFound(new { StatusCode = 404, Message = "الحساب الفرعي غير موجود" });

                var response = new SubAccountResponseDto
                {
                    SubAccountId = updated.SubAccountId ?? 0,
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
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = ex.Message });
            }
        }


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
    }
}
