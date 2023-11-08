using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    using Application.Services;
    using Domain.Entities;
    using Domain.Interfaces.Repositories;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    namespace Tests.Services
    {
        public class AccountOutboxServiceTests
        {
            private readonly Mock<IAccountOutboxRepository> _mockRepository;
            private readonly AccountOutboxService _service;

            public AccountOutboxServiceTests()
            {
                _mockRepository = new Mock<IAccountOutboxRepository>();
                _service = new AccountOutboxService(_mockRepository.Object);
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
            public async Task GetAll_DeveRetornarTodosOsItens()
            {
                // Arrange
                var fakeOutboxItems = new List<AccountOperationPerformedMessageOutbox>
                {
                    CreateAccountOperationPerformedMessageOutboxMock(),
                    CreateAccountOperationPerformedMessageOutboxMock()
                };

                _mockRepository.Setup(repo => repo.GetAll())
                    .ReturnsAsync(fakeOutboxItems);

                // Act
                var items = await _service.GetAll();

                // Assert
                Assert.Equal(fakeOutboxItems.Count, items.Count());
            }

            [Fact]
            public async Task DeleteRangeAsync_DeveChamarDeleteRangeDoRepositorio()
            {
                // Arrange
                var fakeEntities = new List<AccountOperationPerformedMessageOutbox>
                {
                    CreateAccountOperationPerformedMessageOutboxMock(),
                    CreateAccountOperationPerformedMessageOutboxMock()
                };

                _mockRepository.Setup(repo => repo.DeleteRangeAsync(fakeEntities))
                    .Returns(Task.CompletedTask)
                    .Verifiable("DeleteRangeAsync was not called with the correct parameters");

                // Act
                await _service.DeleteRangeAsync(fakeEntities);

                // Assert
                _mockRepository.Verify(repo => repo.DeleteRangeAsync(fakeEntities), Times.Once());
            }
        }
    }

}
