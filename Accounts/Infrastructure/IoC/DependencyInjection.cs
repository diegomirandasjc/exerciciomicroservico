using Application.Services;
using Domain.Interfaces.Repositories;
using Domain.Services;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar o DbContext do Entity Framework
            services.AddDbContext<AccountDBContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("CSF_TRASACTION_CONECTION_STRING")));

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IAccountOutboxRepository, AccountOutboxRepository>();
            services.AddScoped<IAccountOutboxService, AccountOutboxService>();

            return services;
        }
    }

}
