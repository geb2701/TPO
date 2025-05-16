using SharedKernel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Databases
{
    /// <summary>
    /// Interfaz que define las operaciones básicas del repositorio para una entidad.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
    public interface IBaseRepository<TEntity> : IScopedService
    {
        /// <summary>
        /// Obtiene una consulta para la entidad.
        /// </summary>
        IQueryable<TEntity> Query();

        /// <summary>
        /// Obtiene una consulta para la entidad con un filtro.
        /// </summary>
        /// <param name="filter">Expresión de filtro.</param>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Obtiene una consulta para la entidad con un filtro y propiedades incluidas.
        /// </summary>
        /// <param name="filter">Expresión de filtro.</param>
        /// <param name="includes">Propiedades a incluir.</param>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Obtiene una consulta para la entidad con propiedades incluidas.
        /// </summary>
        /// <param name="includes">Propiedades a incluir.</param>
        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Agrega una entidad al repositorio.
        /// </summary>
        /// <param name="entity">Entidad a agregar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task Add(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Agrega un rango de entidades al repositorio.
        /// </summary>
        /// <param name="entity">Entidades a agregar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Tarea que representa la operación asincrónica.</returns>
        Task AddRange(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza una entidad en el repositorio.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Elimina una entidad del repositorio.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Elimina un rango de entidades del repositorio.
        /// </summary>
        /// <param name="entity">Entidades a eliminar.</param>
        void RemoveRange(IEnumerable<TEntity> entity);
    }
}


