using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOperation
    {
        void Execute(Account account, decimal amount, string userId, string userName);
    }
}
