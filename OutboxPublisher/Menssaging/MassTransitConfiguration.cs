using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menssaging
{
    public static class MassTransitConfiguration
    {
        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username(Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER"));
                        h.Password(Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS"));
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
