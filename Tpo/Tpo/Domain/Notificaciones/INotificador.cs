namespace Tpo.Domain.Notificaciones
{
    public interface INotificador
    {
        void Notificar(Usuario.Usuario usuario, string mensaje);
    }
}
