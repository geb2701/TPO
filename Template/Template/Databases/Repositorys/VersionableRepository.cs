using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Databases;
using SharedKernel.Domain.Entity;

namespace Template.Databases.Repositorys;

public interface IVersionableRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>, SharedKernel.Databases.IVersionableRepository<TEntity, TKey>
    where TEntity : VersionableEntity<TKey>
{
}

public abstract class VersionableRepository<TEntity, TKey> : GenericRepository<TEntity, TKey>, IVersionableRepository<TEntity, TKey>
    where TEntity : VersionableEntity<TKey>
{
    private readonly TemplateDbContext _dbContext;

    protected VersionableRepository(TemplateDbContext dbContext) : base(dbContext)
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