using Bussiness.Services.Interfaces;
using DAL.DTO;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services
{
    public class AccountOperationDetailService : BaseService<AccountOperationDetail>, IAccountOperationDetailService
    {
        private readonly IAccountOperationDetailRepository _repository;

        public AccountOperationDetailService(IAccountOperationDetailRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<List<AccountOperationDto>> GetAccountOperationsAsync(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return await _repository.GetAccountOperationsAsync(accountId, startDate, endDate);
        }

        public async Task<List<AccountOperationDetail>> GetAccountOperationsOrderedAsync(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return await _repository.GetAccountOperationsOrderedAsync(accountId, startDate, endDate);
        }
    }
}
