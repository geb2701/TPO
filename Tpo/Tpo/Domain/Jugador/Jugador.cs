using SharedKernel.Domain.Entity;
using Tpo.Domain.Jugador.Models;
using Tpo.Domain.Partido;

namespace Tpo.Domain.Jugador
{
    public class Jugador : BaseEntity<int>, IPartidoObserver
    {
        protected Jugador() { }
        public int UsuarioId { get; set; }
        public int PartidoId { get; set; }
        public Usuario.Usuario Usuario { get; private set; }
        public Partido.Partido Partido { get; private set; }
        public bool Confirmado { get; private set; } = false;

        public static Jugador Create(JugadorForCreation jugadorForCreation)
        {
            var jugador = new Jugador
            {
                Usuario = jugadorForCreation.Usuario,
                Partido = jugadorForCreation.Partido
            };

            jugador.Partido.OnJugadorAgregado(jugador);

            return jugador;
        }

        public void NotificarCambioEstado(string nuevoEstado, Partido.Partido partido)
        {
            Console.WriteLine($"[Jugador: {Usuario.Nombre}] El partido cambió a: {nuevoEstado}");
        }

        public bool Confirmar()
        {
            if (Partido.Estado is not PartidoArmadoState)
                throw new InvalidOperationException("No se puede confirmar un jugador en un partido armado sin haberlo confirmado primero.");
            if (Confirmado)
                return false;
            Confirmado = true;
            Partido.OnJugadorConfirmado();
            return true;
        }
    }
}
