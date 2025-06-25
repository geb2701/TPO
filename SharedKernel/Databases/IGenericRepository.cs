using SharedKernel.Domain.Entity;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Databases
{
    /// <summary>
    /// Interfaz que define las operaciones genéricas del repositorio para una entidad con clave primaria.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public interface IGenericRepository<TEntity, TKey> : IBaseRepository<TEntity>
        where TEntity : Entity<TKey>
    {
        /// <summary>
        /// Obtiene una entidad por su clave primaria o devuelve null si no se encuentra.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <param name="withTracking">Indica si se debe realizar seguimiento de cambios.</param>

        Task<TEntity> GetByIdOrDefault(TKey id, bool withTracking = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene una entidad por su clave primaria o devuelve null si no se encuentra, incluyendo propiedades especificadas.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <param name="withTracking">Indica si se debe realizar seguimiento de cambios.</param>
        /// <param name="includes">Propiedades a incluir.</param>
        Task<TEntity> GetByIdOrDefault(TKey id, bool withTracking = true, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Obtiene una entidad por su clave primaria.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <param name="withTracking">Indica si se debe realizar seguimiento de cambios.</param>
        Task<TEntity> GetById(TKey id, bool withTracking = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene una entidad por su clave primaria, incluyendo propiedades especificadas.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <param name="withTracking">Indica si se debe realizar seguimiento de cambios.</param>
        /// <param name="includes">Propiedades a incluir.</param>
        Task<TEntity> GetById(TKey id, bool withTracking = true, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Verifica si una entidad existe por su clave primaria.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task<bool> Exists(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica si una entidad existe por su clave primaria y lanza una excepción si no se encuentra.
        /// </summary>
        /// <param name="id">Clave primaria de la entidad.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task ExistsWithThrow(TKey id, CancellationToken cancellationToken = default);
    }
}


