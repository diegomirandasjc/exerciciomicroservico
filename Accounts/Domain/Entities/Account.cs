using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account: EntityBase
    {
        public string Description { get; set; }
        public decimal Balance { get; set; }

        public List<AccountMovimentation> Movimentations { get; private set; } = new List<AccountMovimentation>();

        public List<AccountOperationPerformedMessageOutbox> Outbox { get; private set; } = new List<AccountOperationPerformedMessageOutbox>();

        public void AddMovimentation(OperationTypeEnum type, decimal amount, string userId, string userName)
        {
            decimal beforeBalance = Balance;

            Balance += amount;

            Movimentations.Add(new AccountMovimentation
            {
                Amount = amount,
                Timestamp = DateTime.UtcNow,
                OperationType = type,
                UserId = userId,
                UserName = userName,
                BalanceSnapShot = Balance,
                FK_Account = Id,
                Account = this
            });

            Outbox.Add(new AccountOperationPerformedMessageOutbox
            {
                AccountBalanceBeforeOperation = beforeBalance,
                AccountBalanceAfeterOperation = Balance,
                OperationAmount = amount,
                OperationUserId = userId,
                AccountDescription = this.Description,
                OperationUserName = userName,
                OperationDateTime = DateTime.UtcNow
            });                       
        }
    }
}
