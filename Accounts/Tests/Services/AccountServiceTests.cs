using Xunit;
using System;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using Moq;
using Domain.Entities;
using Application.Services;
using Domain.Enums;
using System.Security.Principal;

namespace Tests.Services
{
    public class AccountServiceTests
    {
        private Account GetValidAccount()
        {
            return new Account
            {
                Id = Guid.NewGuid(),
                Description = "Test Account",
                Balance = 100,
            };
        }

        private Mock<IAccountRepository> GetMockForOperation(Account account)
        {
            var mockRepo = new Mock<IAccountRepository>();

            mockRepo.Setup(repo => repo.GetByID(It.IsAny<Guid>())).ReturnsAsync(account);
            mockRepo.Setup(repo => repo.BeginTransaction()).Returns(Task.CompletedTask);
            mockRepo.Setup(repo => repo.CommitTransaction()).Returns(Task.CompletedTask);
            mockRepo.Setup(repo => repo.Update(It.IsAny<Account>())).ReturnsAsync(account);
            mockRepo.Setup(repo => repo.RollbackTransaction()).Returns(Task.CompletedTask);
            mockRepo.Setup(repo => repo.BlockRegisterAccount(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            return mockRepo;
        }

        [Fact]
        public async Task PerformOperation_WhenAccountExists_PerformsOperationSuccessfully()
        {
            // Arrange
            var account = GetValidAccount();
            var mockRepo = GetMockForOperation(account);
            var service = new AccountService(mockRepo.Object);

            // Act
            await service.PerformOperation(account.Id, OperationTypeEnum.Deposit, 100, "userId", "userName");

            // Assert
            mockRepo.Verify(repo => repo.CommitTransaction(), Times.Once);
        }

        [Fact]
        public async Task PerformOperation_WhenAccountDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockRepo = GetMockForOperation(null);
            var service = new AccountService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.PerformOperation(Guid.NewGuid(), OperationTypeEnum.Withdrawal, 100m, "userId", "userName"));
        }

        [Fact]
        public async Task PerformOperation_DepositWithACorrectValue_PerformsOperationSuccessfully()
        {
            // Arrange
            var account = GetValidAccount();
            var mockRepo = GetMockForOperation(account);
            var service = new AccountService(mockRepo.Object);

            // Act
            await service.PerformOperation(account.Id, OperationTypeEnum.Deposit, 23, "userId", "userName");

            // Assert
            mockRepo.Verify(repo => repo.CommitTransaction(), Times.Once);
            Assert.Equal(123, account.Balance);
        }

        [Fact]
        public async Task PerformOperation_DepositWithAIncorrectValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var account = GetValidAccount();
            var mockRepo = GetMockForOperation(account);
            var service = new AccountService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.PerformOperation(account.Id, OperationTypeEnum.Deposit, 0, "userId", "userName"));
            mockRepo.Verify(repo => repo.RollbackTransaction(), Times.Once);
        }

        [Fact]
        public async Task PerformOperation_WithdrawaWithAIncorrectValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var account = GetValidAccount();
            var mockRepo = GetMockForOperation(account);
            var service = new AccountService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.PerformOperation(account.Id, OperationTypeEnum.Withdrawal, 0, "userId", "userName"));
        }

        [Fact]
        public async Task PerformOperation_WithdrawalWithACorrectValue_PerformsOperationSuccessfully()
        {
            // Arrange
            var account = GetValidAccount();
            var mockRepo = GetMockForOperation(account);
            var service = new AccountService(mockRepo.Object);

            // Act
            await service.PerformOperation(account.Id, OperationTypeEnum.Withdrawal, -23, "userId", "userName");

            // Assert
            mockRepo.Verify(repo => repo.CommitTransaction(), Times.Once);
            Assert.Equal(77, account.Balance);
        }

        [Fact]
        public async Task PerformOperation_WithdrawalWithAValueGreaterBalance_ThrowsInvalidOperationException()
        {
            // Arrange
            var account = GetValidAccount();
            var mockRepo = GetMockForOperation(account);
            var service = new AccountService(mockRepo.Object);

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.PerformOperation(account.Id, OperationTypeEnum.Withdrawal, -115, "userId", "userName"));
        }

        [Fact]
        public async Task PerformOperation_WhenOperationFails_RollsBackTransaction()
        {
            // Arrange
            var account = GetValidAccount();
            var mockRepo = GetMockForOperation(account);

            mockRepo.Setup(repo => repo.BlockRegisterAccount(It.IsAny<Guid>())).ThrowsAsync(new Exception("Operation failed"));

            var service = new AccountService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.PerformOperation(account.Id, OperationTypeEnum.Withdrawal, 100m, "userId", "userName"));
            mockRepo.Verify(repo => repo.RollbackTransaction(), Times.Once);
        }
    }
}
