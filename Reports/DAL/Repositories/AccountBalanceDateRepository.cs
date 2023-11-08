using DAL.DTO;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public  class AccountBalanceDateRepository : Repository<AccountBalanceDate>, IAccountBalanceDateRepository
    {
        public AccountBalanceDateRepository(ApplicationDbContext db) : base(db)
        {
           
        }

        
    }
}
