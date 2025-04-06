using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Domain.SharedKernel.Repositories;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository implementation using EF Core.
    /// </summary>
    public abstract class EfRepository<T, TId> : IReadRepository<T,TId>, IWriteRepository<T, TId>
        where T : BaseEntity<TId>
        where TId : IEquatable<TId>
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> FindAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }


        public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification, evaluateCriteriaOnly: true); // Optimization: only apply filters for count
            return await query.CountAsync(cancellationToken);
        }

         public virtual async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
         {
             return await _dbSet.CountAsync(cancellationToken);
         }

         public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
         {
              var query = ApplySpecification(specification, evaluateCriteriaOnly: true); // Optimization: only apply filters
             return await query.AnyAsync(cancellationToken);
         }

         public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
         {
             return await _dbSet.AnyAsync(cancellationToken);
         }


        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual void Update(T entity)
        {
             if (_dbContext.Entry(entity).State == EntityState.Detached)
             {
                 _dbSet.Attach(entity);
             }
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
             if (_dbContext.Entry(entity).State == EntityState.Detached)
             {
                 _dbSet.Attach(entity);
             }
            _dbSet.Remove(entity);
        }

        protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
        {
            return SpecificationEvaluator<T, TId>.GetQuery(_dbSet.AsQueryable(), specification);
        }
    }
}