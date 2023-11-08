using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace Tests.Repositories
{
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    public class AccountRepositoryTests
    {
        private DbContextOptions<AccountDBContext> CreateNewContextOptions()
        {
            var options = new DbContextOptionsBuilder<AccountDBContext>()
                .UseNpgsql(Environment.GetEnvironmentVariable("CONNSTR_ACCOUNT_DEV"))
                .Options;

            using (var context = new AccountDBContext(options))
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }

            return options;
        }

        private Account GetAccountObject(decimal balance = 100)
        {
            return new Account { Id = Guid.NewGuid(), Description = $"TesteCount_{DateTime.UtcNow}", Balance = balance };
        }

        private async Task<Account> CreateAccountAsync(AccountDBContext context, decimal balance = 100)
        {
            var account = GetAccountObject();
            context.Accounts.Add(account);
            await context.SaveChangesAsync();
            return account;
        }


        [Fact]
        public async Task CriarConta_DeveAdicionarContaAoBanco()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Act
            using (var context = new AccountDBContext(options))
            {
                var repository = new AccountRepository(context);
                var newAccount = GetAccountObject();
                await repository.Insert(newAccount);
            }

            // Assert
            using (var context = new AccountDBContext(options))
            {
                var accountExists = await context.Accounts.AnyAsync(a => a.Balance == 200);
                Assert.True(accountExists);
            }
        }

        [Fact]
        public async Task ObterPorID_QuandoContaExiste_RetornaConta()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                var account = await CreateAccountAsync(context);

                // Act
                var repository = new AccountRepository(context);
                var fetchedAccount = await repository.GetByID(account.Id);

                // Assert
                Assert.NotNull(fetchedAccount);
                Assert.Equal(account.Id, fetchedAccount.Id);
            }
        }

        [Fact]
        public async Task AtualizarConta_DeveModificarDetalhesDaConta()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                var account = await CreateAccountAsync(context);
                account.Balance = 200; // Modificar o saldo

                // Act
                var repository = new AccountRepository(context);
                await repository.Update(account);

                // Assert
                var updatedAccount = await context.Accounts.FindAsync(account.Id);
                Assert.Equal(200, updatedAccount.Balance);
            }
        }

        [Fact]
        public async Task DeletarConta_DeveRemoverContaDoBanco()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                var account = await CreateAccountAsync(context);

                // Act
                var repository = new AccountRepository(context);
                await repository.Delete(account);

                // Assert
                var deletedAccount = await context.Accounts.FindAsync(account.Id);
                Assert.Null(deletedAccount);
            }
        }

        [Fact]
        public async Task BloquearRegistro_ExecutarAtualizacao()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var sqlExecuted = string.Empty;

            var mockDbCommand = new Mock<DbCommand>();
            mockDbCommand.SetupGet(cmd => cmd.CommandText).Returns(() => sqlExecuted);
            mockDbCommand.SetupSet(cmd => cmd.CommandText = It.IsAny<string>()).Callback<string>(cmd => sqlExecuted = cmd);
            mockDbCommand.Setup(cmd => cmd.ExecuteNonQueryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                var repository = new AccountRepository(context);
                var exceptionThrown = false;

                // Act
                try
                {
                    await repository.BlockRegisterAccount(accountId);
                }
                catch (Exception)
                {
                    // Se uma exceção é capturada, então o teste deve falhar.
                    exceptionThrown = true;
                }

                Assert.False(exceptionThrown, "Uma exceção foi lançada ao executar BlockRegisterAccount.");
            }
        }

        [Fact]
        public async Task IniciarTransacaoComCommit_CriaNovaTransacaoSemErrosEComCommit()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                var repository = new AccountRepository(context);

                var exceptionThrown = false;

                // Act
                try
                {
                    await repository.BeginTransaction();
                    await repository.CommitTransaction();
                }
                catch (Exception)
                {
                    exceptionThrown = true;
                }

                // Assert
                Assert.False(exceptionThrown, "Uma exceção foi lançada ao iniciar a transação.");
            }
        }

        [Fact]
        public async Task IniciarTransacaoComRollback_CriaNovaTransacaoERollback()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                var repository = new AccountRepository(context);

                var exceptionThrown = false;

                // Act
                try
                {
                    await repository.BeginTransaction();
                    await repository.RollbackTransaction();
                }
                catch (Exception)
                {
                    exceptionThrown = true;
                }

                // Assert
                Assert.False(exceptionThrown, "Uma exceção foi lançada ao iniciar a transação.");
            }
        }


        [Fact]
        public void Descartar_DeveLiberarRecursos()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                var repository = new AccountRepository(context);

                // Act
                repository.Dispose();

                // Assert
                Assert.Throws<ObjectDisposedException>(() =>
                {
                    // Tente realizar uma operação após a eliminação.
                    context.Accounts.ToList();
                });
            }
        }

        [Fact]
        public async Task GetAll_QuandoContasExistem_RetornaTodasAsContas()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                // Adicionar contas de teste ao contexto
                var accounts = new List<Account>
                {
                    GetAccountObject(),
                    GetAccountObject()
                };

                await context.AddRangeAsync(accounts);
                await context.SaveChangesAsync();

                var repository = new AccountRepository(context);

                // Act
                var fetchedAccounts = await repository.GetAll();

                // Assert
                Assert.NotNull(fetchedAccounts);
            }
        }


        [Fact]
        public async Task DeleteRangeAsync_QuandoEntidadesExistem_RemoveEntidades()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new AccountDBContext(options))
            {
                // Adicionar entidades de teste ao contexto
                var entities = new List<Account>
                {
                    GetAccountObject()
                };

                await context.Set<Account>().AddRangeAsync(entities);
                await context.SaveChangesAsync();

                var repository = new AccountRepository(context);

                // Act
                await repository.DeleteRangeAsync(entities);
                await context.SaveChangesAsync();

                // Assert
                var remainingEntities = await context.Set<Account>().ToListAsync();
 
            }
        }
    }
}
