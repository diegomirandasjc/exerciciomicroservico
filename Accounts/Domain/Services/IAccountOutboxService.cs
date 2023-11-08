﻿using Domain.Entities;
using Domain.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IAccountOutboxService : IBaseService<AccountOperationPerformedMessageOutbox>
    {
    }
}
