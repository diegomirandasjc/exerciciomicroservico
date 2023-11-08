using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountOutboxService : BaseService<AccountOperationPerformedMessageOutbox>, IAccountOutboxService
    {
        public AccountOutboxService(IAccountOutboxRepository repository) : base(repository)
        {
            
        }
    }
}
