using Application.Services;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Services
{
    public class TestableBaseService : BaseService<Account>
    {
        public TestableBaseService(IRepository<Account> repository) : base(repository)
        {

        }
    }

    public class BaseServiceTests
    {
        private readonly Mock<IRepository<Account>> _mockRepository;
        private readonly TestableBaseService _baseService;
        private readonly Account _account;

        public BaseServiceTests()
        {
            _mockRepository = new Mock<IRepository<Account>>();
            _baseService = new TestableBaseService(_mockRepository.Object);
            _account = new Account { Id = Guid.NewGuid(), Balance = 100 };
        }

        [Fact]
        public async Task Insert_ShouldCallRepositoryInsertAndReturnResult()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Insert(It.IsAny<Account>())).ReturnsAsync(_account);

            // Act
            var result = await _baseService.Insert(_account);

            // Assert
            _mockRepository.Verify(repo => repo.Insert(_account), Times.Once);
            Assert.Equal(_account, result);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndReturnResult()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Update(It.IsAny<Account>())).ReturnsAsync(_account);

            // Act
            var result = await _baseService.Update(_account);

            // Assert
            _mockRepository.Verify(repo => repo.Update(_account), Times.Once);
            Assert.Equal(_account, result);
        }

        [Fact]
        public async Task GetByID_ShouldCallRepositoryGetByIDAndReturnResult()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByID(_account.Id)).ReturnsAsync(_account);

            // Act
            var result = await _baseService.GetByID(_account.Id);

            // Assert
            _mockRepository.Verify(repo => repo.GetByID(_account.Id), Times.Once);
            Assert.Equal(_account, result);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByID(_account.Id)).ReturnsAsync(_account);
            _mockRepository.Setup(repo => repo.Delete(_account)).Returns(Task.CompletedTask);

            // Act
            await _baseService.Delete(_account.Id);

            // Assert
            _mockRepository.Verify(repo => repo.Delete(_account), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenIdNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var notFoundId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByID(notFoundId))
                           .ReturnsAsync((Account)null); 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _baseService.Delete(notFoundId)
            );
            Assert.Equal("Register not found", exception.Message);
        }



        [Fact]
        public async Task BeginTransaction_ShouldCallRepositoryBeginTransaction()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.BeginTransaction()).Returns(Task.CompletedTask);

            // Act
            await _baseService.BeginTransaction();

            // Assert
            _mockRepository.Verify(repo => repo.BeginTransaction(), Times.Once);
        }

        [Fact]
        public async Task CommitTransaction_ShouldCallRepositoryCommitTransaction()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.CommitTransaction()).Returns(Task.CompletedTask);

            // Act
            await _baseService.CommitTransaction();

            // Assert
            _mockRepository.Verify(repo => repo.CommitTransaction(), Times.Once);
        }

        [Fact]
        public async Task RollbackTransaction_ShouldCallRepositoryRollbackTransaction()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.RollbackTransaction()).Returns(Task.CompletedTask);

            // Act
            await _baseService.RollbackTransaction();

            // Assert
            _mockRepository.Verify(repo => repo.RollbackTransaction(), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldCallRepositoryDispose()
        {
            // Arrange is not needed for Dispose

            // Act
            _baseService.Dispose();

            // Assert
            _mockRepository.Verify(repo => repo.Dispose(), Times.Once);
        }

        [Fact]
        public async Task GetAll_RetornaTodosOsElementos()
        {
            // Arrange
            var entitiesToDelete = new List<Account>
            {
               _account,
               _account
            };

            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(entitiesToDelete);

            // Act
            var result = await _baseService.GetAll();

            // Assert
            Assert.Equal(entitiesToDelete.Count, ((List<Account>)result).Count);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task DeleteRangeAsync_DeletaEntidadesEspecificadas()
        {
            // Arrange
            var entitiesToDelete = new List<Account>
            {
               _account,
               _account
            };

            _mockRepository.Setup(repo => repo.DeleteRangeAsync(entitiesToDelete)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _baseService.DeleteRangeAsync(entitiesToDelete);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteRangeAsync(entitiesToDelete), Times.Once);
        }

    }


}
