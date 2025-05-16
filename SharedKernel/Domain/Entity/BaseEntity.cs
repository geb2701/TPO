using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain.Entity
{
    /// <summary>
    /// Interfaz que define las propiedades y métodos comunes para las entidades base.
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Actualiza las propiedades de creación de la entidad.
        /// </summary>
        /// <param name="createdAt">Fecha de creación.</param>
        /// <param name="createdBy">Creador de la entidad.</param>
        void UpdateCreationProperties(DateTimeOffset createdAt, string createdBy);

        /// <summary>
        /// Actualiza las propiedades de modificación de la entidad.
        /// </summary>
        /// <param name="lastModifiedAt">Última fecha de modificación.</param>
        /// <param name="lastModifiedBy">Último modificador de la entidad.</param>
        void UpdateModifiedProperties(DateTimeOffset lastModifiedAt, string lastModifiedBy);

        /// <summary>
        /// Actualiza el estado de eliminación de la entidad.
        /// </summary>
        /// <param name="isDeleted">Indica si la entidad está eliminada.</param>
        void UpdateIsDeleted(bool isDeleted);

        /// <summary>
        /// Obtiene un valor que indica si la entidad está eliminada.
        /// </summary>
        bool IsDeleted { get; }

        /// <summary>
        /// Obtiene la lista de eventos de dominio asociados a la entidad.
        /// </summary>
        List<DomainEvent> DomainEvents { get; }
    }

    /// <summary>
    /// Clase abstracta que proporciona una implementación base para las entidades.
    /// </summary>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class BaseEntity<TKey> : Entity<TKey>, IBaseEntity
    {
        /// <summary>
        /// Obtiene la fecha de creación de la entidad.
        /// </summary>
        public DateTimeOffset CreatedAt { get; private set; }

        /// <summary>
        /// Obtiene el creador de la entidad.
        /// </summary>
        public string CreatedBy { get; private set; }

        /// <summary>
        /// Obtiene la última fecha de modificación de la entidad.
        /// </summary>
        public DateTimeOffset? LastModifiedAt { get; private set; }

        /// <summary>
        /// Obtiene el último modificador de la entidad.
        /// </summary>
        public string LastModifiedBy { get; private set; }

        /// <summary>
        /// Obtiene un valor que indica si la entidad está eliminada.
        /// </summary>
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// Obtiene la lista de eventos de dominio asociados a la entidad.
        /// </summary>
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        /// <summary>
        /// Actualiza las propiedades de creación de la entidad.
        /// </summary>
        /// <param name="createdAt">Fecha de creación.</param>
        /// <param name="createdBy">Creador de la entidad.</param>
        public void UpdateCreationProperties(DateTimeOffset createdAt, string createdBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
        }

        /// <summary>
        /// Actualiza las propiedades de modificación de la entidad.
        /// </summary>
        /// <param name="lastModifiedAt">Última fecha de modificación.</param>
        /// <param name="lastModifiedBy">Último modificador de la entidad.</param>
        public void UpdateModifiedProperties(DateTimeOffset lastModifiedAt, string lastModifiedBy)
        {
            LastModifiedAt = lastModifiedAt;
            LastModifiedBy = lastModifiedBy;
        }

        /// <summary>
        /// Actualiza el estado de eliminación de la entidad.
        /// </summary>
        /// <param name="isDeleted">Indica si la entidad está eliminada.</param>
        public void UpdateIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }

        /// <summary>
        /// Agrega un evento de dominio a la lista de eventos de dominio de la entidad.
        /// </summary>
        /// <param name="event">Evento de dominio a agregar.</param>
        protected void QueueDomainEvent(DomainEvent @event)
        {
            if (!DomainEvents.Contains(@event))
                DomainEvents.Add(@event);
        }

        /// <summary>
        /// Verifica una regla de negocio y lanza una excepción si la regla está rota.
        /// </summary>
        /// <param name="rule">Regla de negocio a verificar.</param>
        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken)
            {
                throw new BusinessRuleException(rule);
            }
        }
    }
}
