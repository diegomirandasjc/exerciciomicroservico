using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AccountOperationPerformedMessageOutbox
    {
        [Key]
        public Guid Id { get; set; }

        public Guid FK_Account { get; set; }

        public string AccountDescription { get; set; }

        public decimal AccountBalanceBeforeOperation { get; set; }

        public decimal AccountBalanceAfeterOperation { get; set; }

        public DateTime OperationDateTime { get; set; }

        public decimal OperationAmount { get; set; }

        public string OperationUserId { get; set; }

        public string OperationUserName { get; set; }
    }
}
