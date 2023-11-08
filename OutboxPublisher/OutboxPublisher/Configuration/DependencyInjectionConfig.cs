
using Bussiness.Services;
using Bussiness.Services.Interfaces;
using OutboxPublisher.Background;
using System;

namespace OutboxPublisher
{ 
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOutboxPublisherService, OutboxPublisherService>();

            services.AddHostedService<PublisherOutboxService>();

            services.AddHttpClient();

            return services;
        }
    }
}