using System;

namespace Tpo.Domain.Notificaciones
{
    public class NotificadorEmail : INotificador
    {
        public void Notificar(Usuario.Usuario usuario, string mensaje)
        {
            Console.WriteLine($"[Email] enviado a {usuario.Email}: {mensaje}");
        }
    }
}
