using System;

namespace Tpo.Domain.Notificaciones
{
    public class NotificadorPush : INotificador
    {
        public void Notificar(Usuario.Usuario usuario, string mensaje)
        {
            Console.WriteLine($"[Push] enviado al celular de {usuario.Nombre}: {mensaje}");
        }
    }
}
