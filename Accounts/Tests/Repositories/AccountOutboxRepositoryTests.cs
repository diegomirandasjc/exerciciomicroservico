using Microsoft.EntityFrameworkCore;
using Xunit;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Tests.Infrastructure.Data.Repositories
{
    public class AccountOutboxRepositoryTests
    {
        private DbContextOptions<AccountDBContext> CreateNewContextOptions()
        {
            var dbName = $"InMemoryDbForTesting_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<AccountDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return options;
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
        public async Task DeleteRangeAsync_WhenEntitiesExist_RemovesSpecificEntitiesAndKeepsTheRest()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                // Add test entities to the context
                var outboxMessagesToDelete = new List<AccountOperationPerformedMessageOutbox>
                {
                    CreateAccountOperationPerformedMessageOutboxMock(),
                    CreateAccountOperationPerformedMessageOutboxMock(),
                };



                await context.AddRangeAsync(outboxMessagesToDelete);

                await context.SaveChangesAsync();

                var repository = new AccountOutboxRepository(context);

                // Act
                await repository.DeleteRangeAsync(outboxMessagesToDelete);
                await context.SaveChangesAsync();

                // Assert
                var remainingOutboxMessages = await context.Set<AccountOperationPerformedMessageOutbox>().ToListAsync();
                Assert.False(remainingOutboxMessages.Any(m => outboxMessagesToDelete.Select(msg => msg.Id).Contains(m.Id)));

            }
        }

    }
}
