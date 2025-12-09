using Bank_Application.DesignPatterns;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureFacade _featureFacade;

        public FeatureController(IFeatureFacade featureFacade)
        {
            _featureFacade = featureFacade;
        }

        [HttpGet("Get_Features")]
        public async Task<IActionResult> GetFeatures()
        {
            var features = await _featureFacade.ListAllFeatures();
            return Ok(features);
        }

        [HttpPost("Add_Feature")]
        public async Task<IActionResult> AddFeature([FromForm] FeatureCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            var feature = await _featureFacade.CreateFeature(dto.Name, dto.Description);

            var result = new
            {
                FeatureId = feature.FeatureId,
                Name = feature.FeatureName,
                Description = feature.Description
            };

            return Ok(result);
        }





        [HttpDelete("DeleteFeature/{featureId:int}")]
        public async Task<IActionResult> DeleteFeature(int featureId)
        {
            var result = await _featureFacade.RemoveFeature(featureId);

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
            var result = await _featureFacade.LinkFeatureToAccountType(featureId, accountTypeId);

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




        [HttpGet("by-account-type/{accountTypeId}")]
        public async Task<IActionResult> GetFeaturesByAccountType(int accountTypeId)
        {
            var features = await _featureFacade.ListFeaturesOfAccountType(accountTypeId);
            return Ok(features);
        }
    }
}
