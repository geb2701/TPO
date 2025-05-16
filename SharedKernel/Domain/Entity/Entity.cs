using System;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Domain.Entity
{
    /// <summary>
    /// Clase abstracta que representa una entidad con una clave primaria.
    /// </summary>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class Entity<TKey>
    {
        /// <summary>
        /// Obtiene la clave primaria de la entidad.
        /// </summary>
        [Key]
        public TKey Id { get; private set; } = typeof(TKey) == typeof(Guid) ? (TKey)(object)Guid.NewGuid() : default!;
    }
}
