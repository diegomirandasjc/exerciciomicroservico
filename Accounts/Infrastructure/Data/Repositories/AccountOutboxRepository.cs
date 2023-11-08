using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class AccountOutboxRepository : Repository<AccountOperationPerformedMessageOutbox>, IAccountOutboxRepository
    {
        public AccountOutboxRepository(AccountDBContext context) : base(context)
        {
        }
    }
}
