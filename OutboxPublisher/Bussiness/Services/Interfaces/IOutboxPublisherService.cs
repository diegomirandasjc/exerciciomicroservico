using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.Interfaces
{
    public interface IOutboxPublisherService
    {
        Task Publish();
    }
}
