using Domain.Interfaces.Repositories;
using Domain.Services;
using Infrastructure.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void AdicionarInfraestrutura_DeveRegistrarDependencias_Corretamente()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            services.AddInfrastructure(configuration);
            var provider = services.BuildServiceProvider();

            // Assert
            Assert.NotNull(provider.GetService<IAccountRepository>());
            Assert.NotNull(provider.GetService<IAccountService>());
        }
    }
}
