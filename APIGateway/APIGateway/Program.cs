
using APIGateway.Handlers;
using Microsoft.AspNetCore.Authentication;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace APIGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();

            // Configure o HttpClient e associe o JwtTokenValidationHandler
            builder.Services.AddHttpClient("JwtTokenValidation") // "NamedClient" é um exemplo, use um nome apropriado
                .AddHttpMessageHandler<JwtTokenValidationHandler>(); // Associa o JwtTokenValidationHandler

            // Adicione e configure o Ocelot no serviço
            builder.Services.AddOcelot(configuration)
                .AddDelegatingHandler<JwtTokenValidationHandler>(true); // O parâmetro `true` significa que o handler deve ser reutilizado

            // Esquema de autenticação personalizado
            builder.Services.AddAuthentication("CustomScheme")
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", options => { });

            // Registra o JwtTokenValidationHandler como um DelegatingHandler.
            builder.Services.AddTransient<JwtTokenValidationHandler>();

            var app = builder.Build();

            // Antes de chamar o UseOcelot, você pode configurar outros middlewares que você precisa

            // Configure o middleware Ocelot no pipeline de aplicativos
            await app.UseOcelot();

            app.Run();

        }
    }
}