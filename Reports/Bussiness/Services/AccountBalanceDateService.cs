using Bussiness.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services
{
    public class AccountBalanceDateService : BaseService<AccountBalanceDate>, IAccountBalanceDateService
    {
        private readonly IAccountBalanceDateRepository _repository;

        public AccountBalanceDateService(IAccountBalanceDateRepository repository) : base(repository)
        {
            _repository = repository;
        }


    }
}
