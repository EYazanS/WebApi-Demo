using DAL.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IBaseRepository<TEntity, TEntityId> where TEntity : BaseEntity<TEntityId>
    {
        Task<IEnumerable<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>> expression = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

        TEntity GetEntityById(TEntityId entityId);

        void InsertEntity(TEntity entity);

        void DeleteEntity(TEntityId entityId);

        void DeleteEntity(TEntity entity);

        void UpdateEntity(TEntity entity);

        void SaveChanges();

        Task SaveChangesAsync();

        bool HasTransaction();

        void StartTransaction();

        void RollBackTransaction();

        void EndTransaction();
    }

    public class BaseRepository<TEntity, TEntityId> : IBaseRepository<TEntity, TEntityId> where TEntity : BaseEntity<TEntityId>
    {
        private readonly AppDbContext _context;

        private IDbContextTransaction Transaction { get; set; }


        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression is null)
                expression = entity => true;

            return await _context.Set<TEntity>().ToListAsync();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public TEntity GetEntityById(TEntityId entityId) => _context.Set<TEntity>().Find(entityId);

        public void InsertEntity(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public void UpdateEntity(TEntity entity) => _context.Set<TEntity>().Update(entity);

        public void DeleteEntity(TEntityId entityId)
        {
            TEntity entity = GetEntityById(entityId);

            DeleteEntity(entity);
        }

        public void DeleteEntity(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void SaveChanges() => _context.SaveChanges();

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        public void EndTransaction() => Transaction.Commit();

        public void StartTransaction() => Transaction = _context.Database.BeginTransaction();

        public bool HasTransaction() => Transaction != null;

        public void RollBackTransaction() => Transaction.Rollback();
    }
}
