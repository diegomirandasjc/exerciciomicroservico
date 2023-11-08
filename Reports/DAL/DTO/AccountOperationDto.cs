using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class AccountOperationDto
    {
        public Guid AccountId { get; set; }

        public string AccountDescription { get; set; }

        public decimal AccountBalanceAfterOperation { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
