using Bussiness.Services.Interfaces;
using DAL.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menssaging.Consumers
{
    public class OperationPerformedConsumer : IConsumer<IOperationPerformedMessage>
    {
        private readonly IAccountOperationDetailService _service;
        private readonly ILogger<OperationPerformedConsumer> _logger;
        private readonly CacheService _cache;

        public OperationPerformedConsumer(IAccountOperationDetailService service, ILogger<OperationPerformedConsumer> logger, CacheService cache)
        {
            _service = service;
            _logger = logger;
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<IOperationPerformedMessage> context)
        {
            try
            {
                var e = await _cache.GetAsync<bool?>(TCacheKeysUtils.KeyOperation(context.Message.Id.ToString()));

                if (e == null || !e.GetValueOrDefault(false))
                {
                    await _service.Insert(new AccountOperationDetail
                    {
                        Id = context.Message.Id,
                        AccountDescription = context.Message.AccountDescription,
                        AccountBalanceAfeterOperation = context.Message.AccountBalanceAfeterOperation,
                        AccountBalanceBeforeOperation = context.Message.AccountBalanceBeforeOperation,
                        FK_Account = context.Message.AccountId,
                        OperationAmount = context.Message.OperationAmount,
                        OperationDateTime = context.Message.OperationDateTime,
                        OperationUserId = context.Message.OperationUserId,
                        OperationUserName = context.Message.OperationUserName
                    });

                    await _cache.SetAsync(TCacheKeysUtils.KeyOperation(context.Message.Id.ToString()), true, TimeSpan.FromDays(5));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on consume OperationPerformedConsumer");
                throw ex;
            }
        }
    }
}
