using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Operations
{
    public static class TOperationFactory
    {
        public static IOperation GetOperator(OperationTypeEnum operationType)
        {
            switch (operationType)
            {
                case OperationTypeEnum.Deposit:
                    return new OperationDeposit();
                case OperationTypeEnum.Withdrawal:
                    return new OperationWithdrawal();
                default:
                    throw new ArgumentException("Invalid operation type", nameof(operationType));
            }
        }
    }
}
