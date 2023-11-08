using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Services;
using Domain.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    

    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> _repository;

        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity> GetByID(Guid id)
        {
            return await _repository.GetByID(id);
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            return await _repository.Insert(entity);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            return await _repository.Update(entity);
        }

        public async Task Delete(Guid id)
        {
            var entity = await _repository.GetByID(id);

            if (entity == null)
            {
                throw new InvalidOperationException("Register not found");
            }

            await _repository.Delete(entity);
        }

        public async Task BeginTransaction()
        {
            await _repository.BeginTransaction();
        }

        public async Task CommitTransaction()
        {
            await _repository.CommitTransaction();
        }

        public async Task RollbackTransaction()
        {
            await _repository.RollbackTransaction();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            await _repository.DeleteRangeAsync(entities);
        }
    }

}
