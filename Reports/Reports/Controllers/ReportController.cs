using Bussiness.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reports.Controllers.Base;
using System.Security.Principal;

namespace Reports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : BaseController
    {
        private readonly IAccountOperationDetailService _service;

        public ReportController(ILogger<ReportController> logger, IAccountOperationDetailService service) : base(logger)
        {
            _service = service;
        }

        [HttpGet("GetAccountOperationsAsync/{accountId}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetAccountOperationsAsync(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return await HandleOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return HandleValidationErrors();
                }

                return Ok(await _service.GetAccountOperationsAsync(accountId, startDate, endDate));
            });
        }

        [HttpGet("GetAccountOperationsOrderedAsync/{accountId}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetAccountOperationsOrderedAsync(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return await HandleOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return HandleValidationErrors();
                }

                return Ok(await _service.GetAccountOperationsOrderedAsync(accountId, startDate, endDate));
            });
        }
    }
}
