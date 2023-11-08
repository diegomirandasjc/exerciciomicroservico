using DAL.DTO;
using DAL.Interfaces.Repositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IAccountOperationDetailRepository : IRepository<AccountOperationDetail>
    {
        Task<List<AccountOperationDto>> GetAccountOperationsAsync(Guid accountId, DateTime startDate, DateTime endDate);

        Task<List<AccountOperationDetail>> GetAccountOperationsOrderedAsync(Guid accountId, DateTime startDate, DateTime endDate);
    }
}
