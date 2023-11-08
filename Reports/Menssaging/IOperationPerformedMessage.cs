using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menssaging
{
    public interface IOperationPerformedMessage
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string AccountDescription { get; set; }

        public decimal AccountBalanceBeforeOperation  { get; set; }

        public decimal AccountBalanceAfeterOperation { get; set; }

        public DateTime OperationDateTime { get;}

        public decimal OperationAmount { get; set; }

        public string OperationUserId { get; set; }

        public string OperationUserName { get; set; }
    }
}
