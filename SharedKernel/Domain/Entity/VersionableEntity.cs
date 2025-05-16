using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain.Entity
{
    /// <summary>
    /// Interfaz que define las propiedades y métodos para las entidades aprobables.
    /// </summary>
    public interface IVersionableEntity
    {
        /// <summary>
        /// Obtiene la versión actual de la entidad.
        /// </summary>
        int Version { get; }
        /// <summary>
        /// Obtiene la clave de origen de la entidad.
        /// </summary>
        Guid OriginKey { get; }
        /// <summary>
        /// Obtiene o establece la versión máxima de la entidad.
        /// </summary>
        int MaxVersion { get; }
    }
    /// <summary>
    /// Clase abstracta que proporciona una implementación base para las entidades versionables.
    /// </summary>
    /// <typeparam name="T">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class VersionableEntity<T> : BaseEntity<T>, IVersionableEntity
    {
        /// <summary>
        /// Obtiene la versión actual de la entidad.
        /// </summary>
        public int Version { get; private set; } = 1;

        /// <summary>
        /// Obtiene la clave de origen de la entidad.
        /// </summary>
        public Guid OriginKey { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece la versión máxima de la entidad.
        /// </summary>
        [NotMapped]
        public int MaxVersion { get; set; }

        /// <summary>
        /// Incrementa la versión de la entidad.
        /// </summary>
        /// <param name="originKey">Clave de origen de la entidad.</param>
        /// <param name="actualVersion">Versión actual de la entidad.</param>
        protected void NextVersion(Guid originKey, int actualVersion)
        {
            OriginKey = originKey;
            Version = actualVersion + 1;
        }
    }
}


