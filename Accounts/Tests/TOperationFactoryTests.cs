using Domain.Enums;
using Domain.Interfaces;
using Domain.Services.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TOperationFactoryTests
    {
        [Theory]
        [InlineData(OperationTypeEnum.Deposit)]
        [InlineData(OperationTypeEnum.Withdrawal)]
        public void CriarOperacao_QuandoChamadoComTipoValido_RetornaTipoOperacaoCorreto(OperationTypeEnum operationType)
        {
            // Arrange & Act
            var operation = TOperationFactory.GetOperator(operationType);

            // Assert
            Assert.NotNull(operation);
            Assert.IsAssignableFrom<IOperation>(operation);
            if (operationType == OperationTypeEnum.Deposit)
            {
                Assert.IsType<OperationDeposit>(operation);
            }
            else if (operationType == OperationTypeEnum.Withdrawal)
            {
                Assert.IsType<OperationWithdrawal>(operation);
            }
        }

        [Fact]
        public void CriarOperacao_QuandoChamadoComTipoInvalido_LancaArgumentException()
        {
            // Arrange
            var invalidOperationType = (OperationTypeEnum)(-1); // Cast de um valor que não existe no enum

            // Act & Assert
            Assert.Throws<ArgumentException>(() => TOperationFactory.GetOperator(invalidOperationType));
        }
    }
}
