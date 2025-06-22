using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain.Entity
{
    /// <summary>
    /// Interfaz que define las propiedades y m�todos comunes para las entidades base.
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Actualiza las propiedades de creaci�n de la entidad.
        /// </summary>
        /// <param name="createdAt">Fecha de creaci�n.</param>
        /// <param name="createdBy">Creador de la entidad.</param>
        void UpdateCreationProperties(DateTimeOffset createdAt, string createdBy);

        /// <summary>
        /// Actualiza las propiedades de modificaci�n de la entidad.
        /// </summary>
        /// <param name="lastModifiedAt">�ltima fecha de modificaci�n.</param>
        /// <param name="lastModifiedBy">�ltimo modificador de la entidad.</param>
        void UpdateModifiedProperties(DateTimeOffset lastModifiedAt, string lastModifiedBy);

        /// <summary>
        /// Actualiza el estado de eliminaci�n de la entidad.
        /// </summary>
        /// <param name="isDeleted">Indica si la entidad est� eliminada.</param>
        void UpdateIsDeleted(bool isDeleted);

        /// <summary>
        /// Obtiene un valor que indica si la entidad est� eliminada.
        /// </summary>
        bool IsDeleted { get; }
    }

    /// <summary>
    /// Clase abstracta que proporciona una implementaci�n base para las entidades.
    /// </summary>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class BaseEntity<TKey> : Entity<TKey>, IBaseEntity
    {
        /// <summary>
        /// Obtiene la fecha de creaci�n de la entidad.
        /// </summary>
        public DateTimeOffset CreatedAt { get; private set; }

        /// <summary>
        /// Obtiene el creador de la entidad.
        /// </summary>
        public string CreatedBy { get; private set; }

        /// <summary>
        /// Obtiene la �ltima fecha de modificaci�n de la entidad.
        /// </summary>
        public DateTimeOffset? LastModifiedAt { get; private set; }

        /// <summary>
        /// Obtiene el �ltimo modificador de la entidad.
        /// </summary>
        public string LastModifiedBy { get; private set; }

        /// <summary>
        /// Obtiene un valor que indica si la entidad est� eliminada.
        /// </summary>
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// Actualiza las propiedades de creaci�n de la entidad.
        /// </summary>
        /// <param name="createdAt">Fecha de creaci�n.</param>
        /// <param name="createdBy">Creador de la entidad.</param>
        public void UpdateCreationProperties(DateTimeOffset createdAt, string createdBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
        }

        /// <summary>
        /// Actualiza las propiedades de modificaci�n de la entidad.
        /// </summary>
        /// <param name="lastModifiedAt">�ltima fecha de modificaci�n.</param>
        /// <param name="lastModifiedBy">�ltimo modificador de la entidad.</param>
        public void UpdateModifiedProperties(DateTimeOffset lastModifiedAt, string lastModifiedBy)
        {
            LastModifiedAt = lastModifiedAt;
            LastModifiedBy = lastModifiedBy;
        }

        /// <summary>
        /// Actualiza el estado de eliminaci�n de la entidad.
        /// </summary>
        /// <param name="isDeleted">Indica si la entidad est� eliminada.</param>
        public void UpdateIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
