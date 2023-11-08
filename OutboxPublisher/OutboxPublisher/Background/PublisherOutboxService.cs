

using Bussiness.Services.Interfaces;

using MassTransit;

namespace OutboxPublisher.Background
{
    public class PublisherOutboxService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PublisherOutboxService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Publish();

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task Publish()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IOutboxPublisherService>();

            await service.Publish();
        }        
    }
}