namespace Tpo.Domain.Partido
{
    public interface IPartidoObserver
    {
        void NotificarCambioEstado(string nuevoEstado, Partido partido);
    }
}
