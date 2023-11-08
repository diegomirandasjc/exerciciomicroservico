using AccountAPI.Controllers.Base;
using Application.DTO;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _service;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService): base(logger)
        {
            _service = accountService;
        }

        [HttpPost("")]
        public async Task<ActionResult> Insert(Account account)
        {
            return await HandleOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return HandleValidationErrors();
                }

                return Ok(await _service.Insert(account));
            });

        }

        [HttpPut("")]
        public async Task<ActionResult> Update(Account account)
        {
            return await HandleOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return HandleValidationErrors();
                }

                return Ok(await _service.Update(account));
            });           
        }

        [HttpDelete("")]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await HandleOperationAsync(async () =>
            {
                await _service.Delete(id);

                return Ok();
            });
        }

        [HttpPost("Deposit")]
        public async Task<ActionResult> AddDeposit(MovimentationDTO dto)
        {
            return await HandleOperationAsync(async () =>
            {
                await _service.PerformOperation(dto.IdAccount, OperationTypeEnum.Deposit, dto.Amount, GetUserId(), GetUsername());

                return Ok();
            });           
        }

        [HttpPost("Withdrawal")]
        public async Task<ActionResult> AddWithdrawal(MovimentationDTO dto)
        {
            return await HandleOperationAsync(async () =>
            {
                await _service.PerformOperation(dto.IdAccount, OperationTypeEnum.Withdrawal, dto.Amount, GetUserId(), GetUsername());

                return Ok();
            });            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            return await HandleOperationAsync(async () =>
            {
                var account = await _service.GetByID(id);

                if (account == null)
                {
                    return Ok();
                }

                return Ok(account);
            });
        }
    }
}
