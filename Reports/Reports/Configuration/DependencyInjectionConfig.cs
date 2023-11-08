using Bussiness.Services;
using Bussiness.Services.Interfaces;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using System;
using Cache;

namespace Reports
{ 
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.AddScoped<IAccountBalanceDateRepository, AccountBalanceDateRepository>();

            services.AddScoped<IAccountOperationDetailRepository, AccountOperationDetailRepository>();

            services.AddScoped<IAccountBalanceDateService, AccountBalanceDateService>();

            services.AddScoped<IAccountOperationDetailService, AccountOperationDetailService>();

            services.AddSingleton<ICacheStrategy, MemoryCacheStrategy>();

            services.AddSingleton(s => new CacheService(s.GetRequiredService<ICacheStrategy>()));

            return services;
        }
    }
}