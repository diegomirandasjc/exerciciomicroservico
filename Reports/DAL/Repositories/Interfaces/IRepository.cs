using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> Insert(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<TEntity> GetByID(Guid id);

        Task<IEnumerable<TEntity>> GetAll();

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();

        Task Delete(TEntity entity);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
