using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> Insert(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<TEntity> GetByID(Guid id);

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();

        Task Delete(TEntity entity);

        Task<IEnumerable<TEntity>> GetAll();

        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
