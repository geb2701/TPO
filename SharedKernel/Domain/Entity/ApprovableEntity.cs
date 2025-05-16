using System;

namespace SharedKernel.Domain.Entity
{
    /// <summary>
    /// Interfaz que define las propiedades y métodos para las entidades aprobables.
    /// </summary>
    public interface IApprovableEntity
    {
        /// <summary>
        /// Obtiene el estado de la entidad.
        /// </summary>
        EntityStatusType Status { get; }

        /// <summary>
        /// Actualiza las propiedades de aprobación de la entidad.
        /// </summary>
        /// <param name="approvedAt">Fecha de aprobación.</param>
        /// <param name="approvedBy">Aprobador de la entidad.</param>
        void UpdateApprovedProperties(DateTimeOffset approvedAt, string approvedBy);
    }

    /// <summary>
    /// Clase abstracta que proporciona una implementación base para las entidades aprobables.
    /// Las entidades aprobables tienen un estado (status) que cambia según las acciones realizadas sobre ellas.
    /// El flujo de estados es el siguiente:
    /// - Borrador: Estado inicial de la entidad.
    /// - PendienteDeAprobacion: La entidad pasa a este estado desde Borrador.
    /// - Vigente: La entidad pasa a este estado desde PendienteDeAprobacion. En este estado, la entidad no puede ser modificada.
    /// - Rechazado: La entidad pasa a este estado desde PendienteDeAprobacion. En este estado, la entidad puede volver a Borrador.
    /// - Obsoleto: La entidad pasa a este estado desde Vigente.
    /// - PendienteDeEliminacion: Estado especial para marcar la entidad para eliminación.
    /// </summary>
    /// <typeparam name="T">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class ApprovableEntity<T> : BaseEntity<T>, IApprovableEntity
    {
        /// <summary>
        /// Obtiene el estado de la entidad.
        /// </summary>
        public EntityStatusType Status { get; private set; } = EntityStatusType.Borrador;

        /// <summary>
        /// Obtiene la fecha de aprobación de la entidad.
        /// </summary>
        public DateTimeOffset? ApprovedAt { get; private set; }

        /// <summary>
        /// Obtiene el aprobador de la entidad.
        /// </summary>
        public string ApprovedBy { get; private set; }

        /// <summary>
        /// Obtiene los comentarios asociados a la entidad.
        /// </summary>
        public string Comments { get; private set; }

        /// <summary>
        /// Confirma la entidad, cambiando su estado a Vigente.
        /// </summary>
        /// <exception cref="ApplicationException">Lanzada si el estado no es PendienteDeAprobacion.</exception>
        public void Confirmar()
        {
            if (Status != EntityStatusType.PendienteDeAprobacion)
                throw new ApplicationException("Solo es posible aprobar pendientes de aprobación.");
            Status = EntityStatusType.Vigente;
        }

        /// <summary>
        /// Cambia el estado de la entidad a Pendiente de Aprobación.
        /// </summary>
        /// <exception cref="ApplicationException">Lanzada si el estado no es Borrador.</exception>
        public void PasarAPendiente()
        {
            if (Status != EntityStatusType.Borrador)
                throw new ApplicationException("Solo es posible pasar a pendiente borradores.");
            Status = EntityStatusType.PendienteDeAprobacion;
        }

        /// <summary>
        /// Descarta la entidad, cambiando su estado a Rechazado y agregando comentarios.
        /// </summary>
        /// <param name="comments">Comentarios sobre el rechazo.</param>
        /// <exception cref="ApplicationException">Lanzada si el estado no es PendienteDeAprobacion o si los comentarios están vacíos.</exception>
        public void Descartar(string comments)
        {
            if (Status != EntityStatusType.PendienteDeAprobacion)
                throw new ApplicationException("Solo es posible rechazar pendientes de aprobación.");
            if (string.IsNullOrEmpty(comments))
                throw new ApplicationException("Los comentarios son requeridos.");
            Status = EntityStatusType.Rechazado;
            Comments = comments;
        }

        /// <summary>
        /// Cambia el estado de la entidad a Pendiente de Eliminación.
        /// </summary>
        /// <exception cref="ApplicationException">Lanzada si el estado no es PendienteDeEliminacion.</exception>
        public void PendienteDeEliminacion()
        {
            if (Status != EntityStatusType.PendienteDeEliminacion)
                throw new ApplicationException("Solo es posible rechazar pendientes de aprobación.");
            Status = EntityStatusType.Rechazado;
        }

        /// <summary>
        /// Cambia el estado de la entidad a Borrador.
        /// </summary>
        /// <exception cref="ApplicationException">Lanzada si el estado no es Rechazado.</exception>
        public void Reeditar()
        {
            if (Status != EntityStatusType.Rechazado)
                throw new ApplicationException("Solo es posible reeditar rechazados.");
            Status = EntityStatusType.Borrador;
        }

        /// <summary>
        /// Cambia el estado de la entidad a Obsoleto.
        /// </summary>
        /// <exception cref="ApplicationException">Lanzada si el estado no es Vigente.</exception>
        public void PasarAObsoleto()
        {
            if (Status != EntityStatusType.Vigente)
                throw new ApplicationException("Solo es posible pasar a obsoleto vigentes.");
            Status = EntityStatusType.Obsoleto;
        }

        /// <summary>
        /// Valida si es posible actualizar la entidad en su estado actual.
        /// </summary>
        /// <exception cref="ApplicationException">Lanzada si el estado no es Borrador o Rechazado.</exception>
        protected virtual void ValidateUpdate()
        {
            if (Status != EntityStatusType.Borrador && Status != EntityStatusType.Rechazado)
                throw new ApplicationException("No es posible modificar en este estado");
        }

        /// <summary>
        /// Actualiza las propiedades de aprobación de la entidad.
        /// </summary>
        /// <param name="approvedAt">Fecha de aprobación.</param>
        /// <param name="approvedBy">Aprobador de la entidad.</param>
        public void UpdateApprovedProperties(DateTimeOffset approvedAt, string approvedBy)
        {
            ApprovedAt = approvedAt;
            ApprovedBy = approvedBy;
        }
    }
}
