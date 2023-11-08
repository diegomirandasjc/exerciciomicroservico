using API.Controllers;
using Application.DTO;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _mockService;
        private readonly Mock<ILogger<AccountController>> _mockLogger;
        private readonly AccountController _controller;
        private readonly Account _account;

        public AccountControllerTests()
        {
            _mockService = new Mock<IAccountService>();
            _mockLogger = new Mock<ILogger<AccountController>>();
            _controller = new AccountController(_mockLogger.Object, _mockService.Object); // Pass both mocks to the constructor

            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
 
            _account = new Account { Id = Guid.NewGuid(), Balance = 100 };
        }

        [Fact]
        public async Task InserirContaValida_RetornaResultadoOk()
        {
            // Arrange
            var newAccount = new Account { Id = Guid.NewGuid(), Balance = 100 };
            _mockService.Setup(service => service.Insert(It.IsAny<Account>()))
                        .ReturnsAsync(newAccount);

            // Act
            var actionResult = await _controller.Insert(newAccount);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(newAccount, okResult.Value);
        }


        [Fact]
        public async Task InserirModeloInvalido_RetornaRequisicaoInvalida()
        {
            // Arrange
            var newAccount = new Account { Id = Guid.NewGuid(), Balance = 100 };
            _controller.ModelState.AddModelError("Balance", "Required");

            // Act
            var actionResult = await _controller.Insert(newAccount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }


        [Fact]
        public async Task ObterPorID_IdExistentePassado_RetornaResultadoOk()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var account = new Account { Id = testId, Balance = 100 };
            _mockService.Setup(service => service.GetByID(testId))
                        .ReturnsAsync(account);

            // Act
            var actionResult = await _controller.GetById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(account, okResult.Value);
        }


        [Fact]
        public async Task ObterPorID_IdNaoExistentePassado_RetornaNulo()
        {
            // Arrange
            var testId = Guid.NewGuid();
            _mockService.Setup(service => service.GetByID(testId))
                        .ReturnsAsync((Account)null);

            // Act
            var actionResult = await _controller.GetById(testId) as OkObjectResult;

            // Assert
            Assert.Null(actionResult?.Value);
        }

        [Fact]
        public async Task AtualizarContaValida_RetornaResultadoSemConteudo()
        {
            // Arrange
            var testAccount = new Account { Id = Guid.NewGuid(), Balance = 200 };
            _mockService.Setup(service => service.Update(It.IsAny<Account>()))
                        .ReturnsAsync(testAccount);

            // Act
            var result = await _controller.Update(testAccount);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AtualizarModeloInvalido_RetornaRequisicaoInvalida()
        {
            // Arrange
            var testAccount = new Account { Id = Guid.NewGuid(), Balance = 200 };
            _controller.ModelState.AddModelError("Balance", "Required");

            // Act
            var result = await _controller.Update(testAccount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeletarIdExistentePassado_RetornaResultadoOk()
        {
            // Arrange
            var testId = Guid.NewGuid();
            _mockService.Setup(service => service.Delete(testId))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(testId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeletarIdNaoExistentePassado_RetornaExcecaoOperacaoInvalida()
        {
            // Arrange
            var testId = Guid.NewGuid();
            _mockService.Setup(service => service.Delete(testId))
                        .ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await _controller.Delete(testId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AdicionarDeposito_RetornaOk_QuandoDepositoBemSucedido()
        {
            // Arrange
            var dto = new MovimentationDTO
            {
                IdAccount = Guid.NewGuid(),
                Amount = 100.0m
            };
            _mockService.Setup(s => s.PerformOperation(dto.IdAccount, OperationTypeEnum.Deposit, dto.Amount, It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddDeposit(dto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AdicionarRetirada_RetornaOk_QuandoRetiradaBemSucedida()
        {
            // Arrange
            var dto = new MovimentationDTO
            {
                IdAccount = Guid.NewGuid(),
                Amount = 50.0m
            };

            _mockService.Setup(s => s.PerformOperation(dto.IdAccount, OperationTypeEnum.Withdrawal, dto.Amount, It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddWithdrawal(dto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

    }


}
