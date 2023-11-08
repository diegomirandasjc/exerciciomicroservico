using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly AccountDBContext _db;
        protected readonly DbSet<TEntity> _dbSet;
        private IDbContextTransaction _transaction { get; set; }

        public Repository(AccountDBContext context) 
        {
            _db = context;
            _dbSet = _db.Set<TEntity>();
        }

        public void Dispose()
        {
            _db?.Dispose();
        }

        public async Task<TEntity> GetByID(Guid id)
        {
            return await _db.Set<TEntity>().FindAsync(id);
        }

        public async Task Delete(TEntity entity)
        {
            _dbSet.Remove(entity);

            await SaveChanges();
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            var newEntity = _dbSet.Add(entity);
            await SaveChanges();
            return newEntity.Entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            var newEntity = _dbSet.Update(entity);
            await SaveChanges();
            return newEntity.Entity;
        }

        private async Task<int> SaveChanges()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
            _transaction?.Dispose();
        }

        public async Task RollbackTransaction()
        {
            await _db.Database.RollbackTransactionAsync();
            _transaction?.Dispose();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities.Any())
            {
                _db.Set<TEntity>().RemoveRange(entities);

                await _db.SaveChangesAsync();
            }
        }
    }
}
