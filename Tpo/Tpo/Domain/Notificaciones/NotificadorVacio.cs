namespace Tpo.Domain.Notificaciones
{
    public class NotificadorVacio : INotificador
    {
        public void Notificar(Usuario.Usuario usuario, string mensaje)
        {
            // el usuario decide no notificar
        }
    }
}
