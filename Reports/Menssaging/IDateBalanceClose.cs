using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menssaging
{
    public interface IDateBalanceClose
    {
        public Guid Id { get; set; }

        public Guid FK_Account { get; set; }

        public string AccountDescription { get; set; }

        public decimal AccountBalance { get; set; }

        public DateTime Date { get; set; }
    }
}
