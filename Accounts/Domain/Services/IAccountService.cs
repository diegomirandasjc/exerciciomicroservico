using Domain.Entities;
using Domain.Enums;
using Domain.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IAccountService: IBaseService<Account>
    {
        Task PerformOperation(Guid accountId, OperationTypeEnum operationType, decimal amount, string userId, string userName);
    }
}
