using Bank_Application.Models;
using Bank_Application.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank_Application.Controllers
{
    [Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/approvals")]
    public class TransactionApprovalsController:ControllerBase
    {
            private readonly ITransactionApprovalService _service;

            public TransactionApprovalsController(ITransactionApprovalService service)
            {
                _service = service;
            }

            [HttpGet("pending")]
            public async Task<IActionResult> GetPending()
                => Ok(await _service.GetPendingApprovalsAsync());

            [HttpPost("{id}/approve")]
            public async Task<IActionResult> Approve(int id)
                => Ok(await _service.ApproveAsync(id, GetEmployeeId()));

            [HttpPost("{id}/reject")]
            public async Task<IActionResult> Reject(int id)
                => Ok(await _service.RejectAsync(id, GetEmployeeId()));

        private int GetEmployeeId()
        {
            var employeeId = int.Parse(
          User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return employeeId;
        }

    }

}

