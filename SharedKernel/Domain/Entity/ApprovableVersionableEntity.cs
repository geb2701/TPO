using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain.Entity
{
    /// <summary>
    /// Clase abstracta que proporciona una implementaci�n base para las entidades aprobables y versionables.
    /// </summary>
    /// <typeparam name="T">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class ApprovableVersionableEntity<T> : ApprovableEntity<T>, IVersionableEntity
    {
        /// <summary>
        /// Obtiene la versi�n actual de la entidad.
        /// </summary>
        public int Version { get; private set; } = 1;

        /// <summary>
        /// Obtiene la clave de origen de la entidad.
        /// </summary>
        public Guid OriginKey { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece la versi�n m�xima de la entidad.
        /// </summary>
        [NotMapped]
        public int MaxVersion { get; set; }

        /// <summary>
        /// Incrementa la versi�n de la entidad.
        /// </summary>
        /// <param name="originKey">Clave de origen de la entidad.</param>
        /// <param name="actualVersion">Versi�n actual de la entidad.</param>
        protected void NextVersion(Guid originKey, int actualVersion)
        {
            OriginKey = originKey;
            Version = actualVersion + 1;
        }

        /// <summary>
        /// Valida si es posible clonar la entidad en su estado actual.
        /// </summary>
        protected virtual void ValidateClone()
        {
            if (Status != EntityStatusType.Vigente)
                throw new ApplicationException("No es posible clonar en este estado");
        }
    }
}

