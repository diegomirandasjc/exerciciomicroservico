using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.Interfaces
{

    public interface IAccountOperationDetailService : IBaseService<AccountOperationDetail>
    {
        Task<List<AccountOperationDto>> GetAccountOperationsAsync(Guid accountId, DateTime startDate, DateTime endDate);

        Task<List<AccountOperationDetail>> GetAccountOperationsOrderedAsync(Guid accountId, DateTime startDate, DateTime endDate);
    }
}
