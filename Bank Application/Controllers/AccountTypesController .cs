using Bank_Application.DTOs;
using Bank_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypeController : ControllerBase
    {
        private readonly IAccountTypeService _service;
        private readonly IAccountTypeFacade _facade;

        public AccountTypeController(IAccountTypeService service, IAccountTypeFacade facade)
        {
            _service = service;
            _facade = facade;
        }
        [HttpPost("add")]
        public IActionResult Add([FromForm] AccountTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.Add(dto);

            return Ok(new
            {
                status = 200,
                message = "تمت إضافة نوع الحساب بنجاح",
                data = result
            });
        }
        [HttpPut("edit/{id}")]
        public IActionResult Edit(int id, [FromForm] EditAccountTypeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Edit(id, dto);

            if (result == null)
                return NotFound(new { status = 404, message = "نوع الحساب غير موجود" });

            return Ok(new
            {
                status = 200,
                message = "تم تعديل نوع الحساب بنجاح",
                data = result
            });
        }
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var success = _facade.Delete(id);
            if (!success)
                return NotFound(new { status = 404, message = "نوع الحساب غير موجود" });

            return Ok(new
            {
                status = 200,
                message = "تم حذف نوع الحساب بنجاح"
            });
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var result = _facade.GetAll();
            return Ok(new
            {
                status = 200,
                message = "جميع أنواع الحسابات",
                data = result
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _facade.GetById(id);
            if (result == null)
                return NotFound(new { status = 404, message = "نوع الحساب غير موجود" });

            return Ok(new
            {
                status = 200,
                message = "بيانات نوع الحساب",
                data = result
            });
        }
    }
}
