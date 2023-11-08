
using Bussiness.Services;
using Bussiness.Services.Interfaces;


using System;

namespace API.Controllers.V1
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<BancoAPIContext>();

            services.AddScoped<ITokenService, TokenService>();
            

            return services;
        }


    }
}