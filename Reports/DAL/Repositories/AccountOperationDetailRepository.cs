using DAL.DTO;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AccountOperationDetailRepository : Repository<AccountOperationDetail>, IAccountOperationDetailRepository
    {
        public AccountOperationDetailRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<List<AccountOperationDto>> GetAccountOperationsAsync(Guid accountId, DateTime startDate, DateTime endDate)
        {
            var query = _dbSet
                .Where(a => a.FK_Account == accountId && a.OperationDateTime.Date >= startDate.Date && a.OperationDateTime.Date <= endDate.Date)
                .GroupBy(a => new
                {
                    AccountId = a.FK_Account,
                    AccountDescription = a.AccountDescription, 
                    Date = a.OperationDateTime.Date
                })
                .Select(g => new
                {
                    AccountId = g.Key.AccountId,
                    AccountDescription = g.Key.AccountDescription,
                    OperationDate = g.Key.Date,
                    LastOperation = g.OrderByDescending(a => a.OperationDateTime).FirstOrDefault()
                })
                .Select(g => new AccountOperationDto
                {
                    AccountId = g.AccountId,
                    AccountDescription = g.AccountDescription,
                    AccountBalanceAfterOperation = g.LastOperation.AccountBalanceAfeterOperation, 
                    OperationDate = g.OperationDate
                })
                .OrderByDescending(a => a.OperationDate);

            return await query.ToListAsync();
        }


        public async Task<List<AccountOperationDetail>> GetAccountOperationsOrderedAsync(Guid accountId, DateTime startDate, DateTime endDate)
        {
            var query = _dbSet
                .Where(a => a.FK_Account == accountId && a.OperationDateTime >= startDate && a.OperationDateTime <= endDate)
                .OrderBy(a => a.OperationDateTime); // Ordena pelo DateTime completo, incluindo a hora.

            return await query.ToListAsync();
        }
    }
}
