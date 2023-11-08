using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Data.Context
{
    public class AccountDBContext : DbContext
    {
        public DbSet<AccountMovimentation> AccountsMovimentations { get; set; }
        public DbSet<AccountOperationPerformedMessageOutbox> AccountOperationPerformedMessageOutbox { get; set; }
        
        public DbSet<Account> Accounts { get; set; }

        public AccountDBContext(DbContextOptions<AccountDBContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
