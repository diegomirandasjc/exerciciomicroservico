using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Operations
{
    public class OperationWithdrawal : IOperation
    {
        public void Execute(Account account, decimal amount, string userId, string userName)
        {
            if (amount >= 0)
            {
                throw new InvalidOperationException("Value to deposit must be less than 0.");
            }

            if (account.Balance < Math.Abs(amount))
            {
                throw new InvalidOperationException("Insufficient funds.");
            }

            account.AddMovimentation(Domain.Enums.OperationTypeEnum.Withdrawal, amount, userId, userName);
        }
    }
}
