namespace Bank_Application.Controllers
{
    using Bank_Application.services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")] 
    public class TransactionReportController : ControllerBase
    {
        private readonly ITransactionReportService _reportService;

        public TransactionReportController(ITransactionReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime? date)
        {
            var reportDate = date ?? DateTime.Now;
            var pdfBytes = await _reportService.GenerateDailyReportAsync(reportDate);

            var fileName = $"TransactionReport_{reportDate:yyyyMMdd}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }

}
