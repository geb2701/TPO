using SharedKernel.Domain.Entity;
using Tpo.Domain.Jugador.Models;
using Tpo.Domain.Partido;

namespace Tpo.Domain.Jugador
{
    public class Jugador : BaseEntity<int>, IPartidoObserver
    {
        protected Jugador() { }

        public Usuario.Usuario Usuario { get; private set; }
        public Partido.Partido Partido { get; private set; }
        public bool Confirmado { get; private set; } = false;

        public static Jugador Create(JugadorForCreation jugadorForCreation)
        {
            return new Jugador
            {
                Usuario = jugadorForCreation.Usuario,
                Partido = jugadorForCreation.Partido
            };
        }

        public void NotificarCambioEstado(string nuevoEstado, Partido.Partido partido)
        {
            // Tenemos que poner un adapter
            Console.WriteLine($"[Jugador: {Usuario.Nombre}] El partido cambió a: {nuevoEstado}");
        }

        public bool Confirmar()
        {
            if (Partido.Estado is not PartidoArmadoState)
                throw new InvalidOperationException("No se puede confirmar un jugador en un partido armado sin haberlo confirmado primero.");
            if (Confirmado)
                return false;
            Confirmado = true;
            return true;
        }
    }
}
