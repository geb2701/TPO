using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Databases;
using SharedKernel.Domain.Entity;
using System.Linq.Expressions;
using Tpo.Exceptions;

namespace Tpo.Databases.Repositorys;

/// <summary>
/// Interfaz que define las operaciones del repositorio con consultas que incluyen propiedades relacionadas.
/// </summary>
/// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
/// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
public interface IRepositoryIncludableQueryable<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    /// <summary>
    /// Obtiene una entidad por su clave primaria o devuelve null si no se encuentra, incluyendo propiedades especificadas.
    /// </summary>
    /// <param name="id">Clave primaria de la entidad.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <param name="withTracking">Indica si se debe realizar seguimiento de cambios.</param>
    /// <param name="includes">Propiedades a incluir.</param>
    Task<TEntity> GetByIdOrDefault(TKey id, CancellationToken cancellationToken = default,
        bool withTracking = true,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes);

    /// <summary>
    /// Obtiene una entidad por su clave primaria, incluyendo propiedades especificadas.
    /// </summary>
    /// <param name="id">Clave primaria de la entidad.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <param name="withTracking">Indica si se debe realizar seguimiento de cambios.</param>
    /// <param name="includes">Propiedades a incluir.</param>
    Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default,
        bool withTracking = true, params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes);

    IQueryable<TEntity> Query(
        Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default,
        bool withTracking = true,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes);
}
public abstract class GenericRepository<TEntity, TKey>(TpoDbContext dbContext) : BaseRepository<TEntity>(dbContext), IRepositoryIncludableQueryable<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    public virtual async Task<TEntity> GetByIdOrDefault(TKey id,
        CancellationToken cancellationToken = default, bool withTracking = true)
    {
        return withTracking
            ? await dbContext.Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken)
            : await dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }
    public virtual async Task<TEntity> GetByIdOrDefault(TKey id, CancellationToken cancellationToken = default,
        bool withTracking = true,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
    {
        var query = withTracking
            ? dbContext.Set<TEntity>()
            : dbContext.Set<TEntity>().AsNoTracking();

        if (includes != null)
            foreach (var includeExpression in includes)
                query = includeExpression(query);

        return await query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }
    public virtual async Task<TEntity> GetByIdOrDefault(TKey id, CancellationToken cancellationToken = default,
        bool withTracking = true, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = withTracking
            ? dbContext.Set<TEntity>()
            : dbContext.Set<TEntity>().AsNoTracking();

        if (includes != null)
            foreach (var includeExpression in includes)
                query = query.Include(includeExpression);

        return await query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }
    public virtual async Task<TEntity> GetById(TKey id,
        CancellationToken cancellationToken = default, bool withTracking = true)
    {
        var entity = await GetByIdOrDefault(id, cancellationToken, withTracking);

        if (entity == null)
            throw new NotFoundException($"{typeof(TEntity).Name} con ID '{id}' no fue encontrado.");

        return entity;
    }
    public virtual async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default, bool withTracking = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var entity = await GetByIdOrDefault(id, cancellationToken, withTracking, includes);

        if (entity == null)
            throw new NotFoundException($"{typeof(TEntity).Name} con ID '{id}' no fue encontrado.");

        return entity;
    }
    public virtual async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default,
        bool withTracking = true, params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
    {
        var entity = await GetByIdOrDefault(id, cancellationToken, withTracking, includes);

        if (entity == null)
            throw new NotFoundException($"{typeof(TEntity).Name} con ID '{id}' no fue encontrado.");

        return entity;
    }

    public virtual async Task<bool> Exists(TKey id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>()
            .AnyAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public virtual async Task ExistsWithThrow(TKey id, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Set<TEntity>().AnyAsync(e => e.Id.Equals(id), cancellationToken);
        if (!exists) throw new NotFoundException($"{typeof(TEntity).Name} con ID '{id}' no fue encontrado.");

    }

    public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default,
        bool withTracking = true, params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
    {
        var query = withTracking
            ? dbContext.Set<TEntity>()
            : dbContext.Set<TEntity>().AsNoTracking();

        if (includes != null)
            foreach (var includeExpression in includes)
                query = includeExpression(query);

        return query;
    }
}