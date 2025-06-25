namespace Tpo.Domain.Jugador
{
    public interface IJugadorObserver
    {
        void OnJugadorConfirmado();
        void OnJugadorAgregado(Jugador jugador);
    }
}
