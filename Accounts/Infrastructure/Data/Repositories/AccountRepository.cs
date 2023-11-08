using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(AccountDBContext context) : base(context)
        {
        }

        public async Task BlockRegisterAccount(Guid id)
        {
            var parameter = new NpgsqlParameter("id", id);
            await _db.Database.ExecuteSqlRawAsync("SELECT * FROM \"Accounts\" WHERE \"Id\" = @id FOR UPDATE", parameter);
        }
    }
}
