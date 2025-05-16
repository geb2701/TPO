using System.ComponentModel;

namespace SharedKernel.Domain.Entity
{
    public enum EntityStatusType
    {
        [Description("Pendiente De Eliminación")]
        PendienteDeEliminacion = -3,
        [Description("Rechazado")]
        Rechazado = -2,
        [Description("Obsoleto")]
        Obsoleto = -1,
        [Description("Borrador")]
        Borrador = 0,
        [Description("Vigente")]
        Vigente = 1,
        [Description("Pendiente De Aprobación")]
        PendienteDeAprobacion = 2
    }
}