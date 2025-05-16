using SharedKernel.Domain.Entity;

namespace SharedKernel.Databases
{

    /// <summary>
    /// Interfaz que define las operaciones del repositorio para entidades versionables.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public interface IApprovableVersionableRepository<TEntity, TKey>
        where TEntity : ApprovableVersionableEntity<TKey>
    {
        /// <summary>
        /// Obtiene la entidad con la versión máxima.
        /// </summary>
        /// <param name="entity">Entidad para la cual se obtendrá la versión máxima.</param>
        /// <returns>Entidad con la versión máxima.</returns>
        TEntity GetMaxVersion(TEntity entity);
    }
}
