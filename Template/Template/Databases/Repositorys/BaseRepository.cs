using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Databases;
using System.Linq.Expressions;

namespace Template.Databases.Repositorys
{
    public abstract class BaseRepository<TEntity>(TemplateDbContext dbContext) : IBaseRepository<TEntity>
        where TEntity : class
    {
        public virtual IQueryable<TEntity> Query()
        {
            return dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> result = dbContext.Set<TEntity>();

            if (filter != null) result = result.Where(filter);

            return result;
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (includes != null)
                foreach (var includeExpression in includes)
                    query = query.Include(includeExpression);

            return query;
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter,
            params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (filter != null) query = query.Where(filter);

            if (includes != null)
                foreach (var includeExpression in includes)
                    query = includeExpression(query);

            return query;
        }

        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (includes != null)
                foreach (var includeExpression in includes)
                    query = query.Include(includeExpression);

            return query;
        }

        public IQueryable<TEntity> Query(params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (includes != null)
                foreach (var includeExpression in includes)
                    query = includeExpression(query);

            return query;
        }

        public virtual async Task Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public virtual async Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
        }
    }
}
