using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.Interfaces
{
    public interface IBaseService<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> Insert(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<TEntity> GetByID(Guid id);

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();

        Task Delete(Guid id);

        Task<IEnumerable<TEntity>> GetAll();

        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
