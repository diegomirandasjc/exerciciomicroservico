using AccountAPI.Controllers.Base;
using API.Controllers;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutboxController : BaseController
    {
        private readonly IAccountOutboxService _service;

        public OutboxController(ILogger<AccountController> logger, IAccountOutboxService service) : base(logger)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return await HandleOperationAsync(async () =>
            {
                return Ok(await _service.GetAll());
            });

        }

        [HttpPost("DeleteAll")]
        public async Task<ActionResult> DeleteAll([FromBody] IEnumerable<AccountOperationPerformedMessageOutbox> entities)
        {
            return await HandleOperationAsync(async () =>
            {
                await _service.DeleteRangeAsync(entities);
                return Ok();
            });

        }
    }
}
