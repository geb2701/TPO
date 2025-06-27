using System.ComponentModel;

namespace Tpo.Domain
{
    public enum TipoNotificacion
    {
        [Description("Ninguna notificación")]
        Ninguna = 0,

        [Description("Por correo electrónico")]
        Email = 1,

        [Description("Notificación push al celular")]
        PushCelular = 2,

        [Description("Email y notificación push")]
        EmailYPush = 4,

        [Description("Todas las notificaciones disponibles")]
        Todas = 5
    }
}
