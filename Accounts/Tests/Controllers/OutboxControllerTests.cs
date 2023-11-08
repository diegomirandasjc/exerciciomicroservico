using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Domain.Services;
using Domain.Entities;
using AccountAPI.Controllers;
using API.Controllers;

namespace AccountAPI.Tests
{
    public class OutboxControllerTests
    {
        private readonly Mock<IAccountOutboxService> _mockService;
        private readonly Mock<ILogger<AccountController>> _mockLogger;
        private readonly OutboxController _controller;

        public OutboxControllerTests()
        {
            _mockService = new Mock<IAccountOutboxService>();
            _mockLogger = new Mock<ILogger<AccountController>>();
            _controller = new OutboxController(_mockLogger.Object, _mockService.Object);
        }

        public AccountOperationPerformedMessageOutbox CreateAccountOperationPerformedMessageOutboxMock()
        {
            return new AccountOperationPerformedMessageOutbox
            {
                Id = Guid.NewGuid(),
                FK_Account = Guid.NewGuid(),
                AccountDescription = "Test Account Description",
                AccountBalanceBeforeOperation = 1000.00m,
                AccountBalanceAfeterOperation = 1200.00m,
                OperationDateTime = DateTime.UtcNow,
                OperationAmount = 200.00m,
                OperationUserId = "TestUser",
                OperationUserName = "Test UserName"
            };
        }


        [Fact]
        public async Task GetAll_QuandoChamado_RetornaActionResult()
        {
            // Arrange
            _mockService.Setup(service => service.GetAll())
                .ReturnsAsync(new List<AccountOperationPerformedMessageOutbox>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_QuandoServiceLancaExcecao_RetornaStatusCode500()
        {
            // Arrange
            _mockService.Setup(service => service.GetAll())
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAll();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }


        [Fact]
        public async Task DeleteAll_QuandoChamadoComEntidadesValidas_RetornaOk()
        {
            // Arrange
            var entities = new List<AccountOperationPerformedMessageOutbox>
            {
                CreateAccountOperationPerformedMessageOutboxMock(), // Adiciona uma entidade mock
                CreateAccountOperationPerformedMessageOutboxMock()  // Adiciona outra entidade mock
            };

            _mockService.Setup(service => service.DeleteRangeAsync(entities))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAll(entities);

            // Assert
            Assert.IsType<OkResult>(result);
        }


    }
}
