using Bussiness.Services.Interfaces;
using DAL.Models;

using MassTransit;
using Menssaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services
{
    public class OutboxPublisherService : IOutboxPublisherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _dataUrl;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger _logger;

        public OutboxPublisherService( HttpClient httpClient, IPublishEndpoint publishEndpoint, IConfiguration configuration, ILogger<OutboxPublisherService> logger)  
        {
            _publishEndpoint = publishEndpoint;

            _httpClient = httpClient;

            _dataUrl = configuration["EndpointSettings:DataUrl"];

            _logger = logger;
        }

        public async Task Publish()
        {
            try
            {
                var response = await _httpClient.GetAsync(_dataUrl);

                List<AccountOperationPerformedMessageOutbox> list = null;

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<AccountOperationPerformedMessageOutbox>>(content);

                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            await SendMessage(item);
                        }

                        if (list.Any())
                        {
                            string jsonContent = JsonConvert.SerializeObject(list);

                            using var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            var postResponse = await _httpClient.PostAsync(_dataUrl + "/DeleteAll", stringContent);

                            if (!postResponse.IsSuccessStatusCode)
                            {
                                string responseContent = await postResponse.Content.ReadAsStringAsync();
                                throw new Exception($"Error on post delete-{postResponse.StatusCode} {responseContent} {stringContent}");
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Error on publish");
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error on publish method");
            }
            
        }


        private async Task SendMessage(AccountOperationPerformedMessageOutbox outbox)
        {
            await _publishEndpoint.Publish<IOperationPerformedMessage>(new
            {
                Id = outbox.Id,
                AccountId = outbox.FK_Account,
                AccountDescription = outbox.AccountDescription,
                AccountBalanceBeforeOperation = outbox.AccountBalanceBeforeOperation,
                AccountBalanceAfeterOperation = outbox.AccountBalanceAfeterOperation,
                OperationDateTime = outbox.OperationDateTime,
                OperationAmount = outbox.OperationAmount,
                OperationUserId = outbox.OperationUserId,
                OperationUserName = outbox.OperationUserName
            });
        }
    }
}
