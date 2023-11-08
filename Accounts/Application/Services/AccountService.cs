using Application.Services.Base;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Services;
using Domain.Services.Base;
using Domain.Services.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository): base(accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task PerformOperation(Guid accountId, OperationTypeEnum operationType, decimal amount, string userId, string userName)
        {
            Account account = await _accountRepository.GetByID(accountId);

            if (account == null)
            {
                throw new InvalidOperationException("Account not found.");                
            }

            var operation = TOperationFactory.GetOperator(operationType);

            try
            {
                await _accountRepository.BeginTransaction();

                await _accountRepository.BlockRegisterAccount(accountId);
                
                operation.Execute(account, amount, userId, userName);

                await _accountRepository.Update(account);

                await _accountRepository.CommitTransaction();
            }
            catch (Exception)
            {
                await _accountRepository.RollbackTransaction();

                throw;
            }            
        }
    }
}
