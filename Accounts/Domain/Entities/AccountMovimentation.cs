using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AccountMovimentation: EntityBase
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }

        public OperationTypeEnum OperationType { get; set; }

        public decimal Amount { get; set; }

        public decimal BalanceSnapShot { get; set; }

        [ForeignKey("Account")]
        public Guid FK_Account { get; set; }
        public Account Account { get; set; }
    }
}
