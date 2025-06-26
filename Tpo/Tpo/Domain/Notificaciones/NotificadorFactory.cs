using Tpo.Domain.Usuario;

namespace Tpo.Domain.Notificaciones
{
    public static class NotificadorFactory
    {
        public static INotificador CrearNotificador(TipoNotificacion tipoNotificacion)
        {
            return tipoNotificacion switch
            {
                TipoNotificacion.Email => new NotificadorEmail(),
                TipoNotificacion.PushCelular => new NotificadorPush(),
                TipoNotificacion.EmailYPush => new NotificadorMultiple(new INotificador[] {
                    new NotificadorEmail(), new NotificadorPush() }),
                _ => new NotificadorVacio(),
            };
        }
    }
}
