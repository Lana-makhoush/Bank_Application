using Bank_Application.DesignPatterns;
using Bank_Application.DesignPatterns.Decorator;
using Bank_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureDecorator _featuredecor;
        private readonly IAccountService _accountService;

        public FeatureController(IFeatureDecorator featuredecorator, IAccountService accountService)
        {
            _featuredecor = featuredecorator;
            _accountService = accountService;
        }
        [Authorize(Roles = "Manager,Admin,Teller")]

        [HttpGet("Get_Features")]
        public async Task<IActionResult> GetFeatures()
        {
            var features = await _featuredecor.ListAllFeatures();
            return Ok(features);
        }
        [Authorize(Roles = "Manager")]

        [HttpPost("Add_Feature")]
        public async Task<IActionResult> AddFeature([FromForm] FeatureCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Cost < 0)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "الكلفة يجب أن تكون رقمًا موجبًا"
                });
            }

            var feature = await _featuredecor.CreateFeature(
                dto.Name,
                dto.Description,
                dto.Cost
            );

            return Ok(new
            {
                FeatureId = feature.FeatureId,
                Name = feature.FeatureName,
                Description = feature.Description,
                Cost = feature.Cost
            });
        }








        [HttpDelete("DeleteFeature/{featureId:int}")]
        public async Task<IActionResult> DeleteFeature(int featureId)
        {
            var result = await _featuredecor.RemoveFeature(featureId);

            if (!result)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "لا يمكن حذف هذه الميزة لأنها إما غير موجودة أو مرتبطة بنوع حساب"
                });
            }

            return Ok(new
            {
                status = 200,
                message = "تم حذف الميزة بنجاح"
            });
        }

        [HttpPost("assign-Feature-to-AccountType/{featureId:int}/{accountTypeId:int}")]
        public async Task<IActionResult> AssignFeature(int featureId, int accountTypeId)
        {
            var result = await _featuredecor.LinkFeatureToAccountType(featureId, accountTypeId);

            if (!result)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = " هذه الميزة مضافة مسبقًا لنوع الحساب المحدد"
                });
            }

            return Ok(new
            {
                status = 200,
                message = " تم ربط الميزة بنوع الحساب بنجاح",
              
            });
        }

        [HttpPost("DeductFeatures")]
        public async Task<IActionResult> DeductFeaturesFromAllAccounts()
        {
            var resultList = new List<object>();

            // 1️⃣ كل الحسابات الرئيسية
            var clientAccounts = await _accountService.GetAllClientAccounts();
            foreach (var ca in clientAccounts)
            {
                if (ca.Account?.AccountTypeId != null)
                {
                    var features = await _accountService.GetFeaturesByAccountType(ca.Account.AccountTypeId.Value);

                    decimal initialBalance = ca.Balance ?? 0m;
                    decimal balanceAfter = initialBalance;

                    var featureDeductions = new List<object>();

                    foreach (var feature in features)
                    {
                        decimal cost = feature.Cost; // Cost أصبح decimal مباشرة
                        balanceAfter -= cost;
                        if (balanceAfter < 0) balanceAfter = 0;

                        featureDeductions.Add(new
                        {
                            feature.FeatureName,
                            Cost = cost
                        });
                    }

                    // تحديث الرصيد بعد الاقتطاع
                    ca.Balance = balanceAfter;
                    await _accountService.UpdateClientAccountBalance(ca.Id, balanceAfter);

                    resultList.Add(new
                    {
                        Type = "ClientAccount",
                        AccountId = ca.Id,
                        ClientName = $"{ca.Client?.FirstName} {ca.Client?.LastName}",
                        InitialBalance = initialBalance,
                        BalanceAfterDeduction = balanceAfter,
                        Features = featureDeductions
                    });
                }
            }

            // 2️⃣ كل الحسابات الفرعية
            var subAccounts = await _accountService.GetAllSubAccounts();
            foreach (var sa in subAccounts)
            {
                if (sa.SubAccountTypeId != null)
                {
                    var features = await _accountService.GetFeaturesByAccountType(sa.SubAccountTypeId.Value);

                    decimal initialBalance = sa.Balance ?? 0m;
                    decimal balanceAfter = initialBalance;

                    var featureDeductions = new List<object>();

                    foreach (var feature in features)
                    {
                        decimal cost = feature.Cost; 
                        balanceAfter -= cost;
                        if (balanceAfter < 0) balanceAfter = 0;

                        featureDeductions.Add(new
                        {
                            feature.FeatureName,
                            Cost = cost
                        });
                    }

               
                    sa.Balance = balanceAfter;
                    await _accountService.UpdateSubAccountBalance(sa.SubAccountId, balanceAfter);

                    resultList.Add(new
                    {
                        Type = "SubAccount",
                        AccountId = sa.SubAccountId,
                        InitialBalance = initialBalance,
                        BalanceAfterDeduction = balanceAfter,
                        Features = featureDeductions
                    });
                }
            }

            return Ok(new
            {
                status = 200,
                message = "تم تنفيذ الاقتطاع بنجاح",
                data = resultList
            });
        }



    }
}
