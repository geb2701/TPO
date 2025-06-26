namespace Tpo.Domain.Notificaciones
{
    public class NotificadorMultiple : INotificador
    {
        private readonly IEnumerable<INotificador> _notificadores;

        public NotificadorMultiple(IEnumerable<INotificador> notificadores)
        {
            _notificadores = notificadores;
        }

        public void Notificar(Usuario.Usuario usuario, string mensaje)
        {
            foreach (var notificador in _notificadores)
                notificador.Notificar(usuario, mensaje);
        }
    }
}
