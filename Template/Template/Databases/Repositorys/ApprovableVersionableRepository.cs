using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Domain.Entity;

namespace Template.Databases.Repositorys
{
    public interface IApprovableVersionableRepository<TEntity, TKey> : SharedKernel.Databases.IApprovableVersionableRepository<TEntity, TKey> where TEntity : ApprovableVersionableEntity<TKey>
    {
    }
    public abstract class ApprovableVersionableRepository<TEntity, TKey> : ApprovableRepository<TEntity, TKey>, IApprovableVersionableRepository<TEntity, TKey> where TEntity : ApprovableVersionableEntity<TKey>
    {
        private readonly TemplateDbContext _dbContext;

        protected ApprovableVersionableRepository(TemplateDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default, bool withTracking = true)
        {
            var result = await base.GetById(id, cancellationToken, withTracking);
            if (result != null)
                GetMaxVersion(result);
            return result;
        }

        public override async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default,
            bool withTracking = true, params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
        {
            var result = await base.GetById(id, cancellationToken, withTracking, includes);
            if (result != null)
                GetMaxVersion(result);
            return result;
        }

        public override async Task<TEntity> GetByIdOrDefault(TKey id, CancellationToken cancellationToken = default, bool withTracking = true)
        {
            var result = await base.GetByIdOrDefault(id, cancellationToken, withTracking);
            if (result != null)
                GetMaxVersion(result);
            return result;
        }

        public override async Task<TEntity> GetByIdOrDefault(TKey id, CancellationToken cancellationToken = default,
            bool withTracking = true, params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
        {
            var result = await base.GetByIdOrDefault(id, cancellationToken, withTracking, includes);
            if (result != null)
                GetMaxVersion(result);
            return result;
        }

        public virtual TEntity GetMaxVersion(TEntity entity)
        {
            entity.MaxVersion = _dbContext.Set<TEntity>().Where(x => x.OriginKey == entity.OriginKey).Max(x => x.Version);

            return entity;
        }
    }
}
