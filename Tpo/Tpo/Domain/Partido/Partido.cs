using SharedKernel.Domain.Entity;
using Tpo.Domain.Partido.Models;

namespace Tpo.Domain.Partido
{
    public class Partido : BaseEntity<int>
    {
        protected Partido() { }
        public IPartidoState Estado { get; private set; } = new NecesitamosJugadoresState();
        public DateTime FechaHora { get; private set; }
        public TimeSpan Duracion { get; private set; } = TimeSpan.FromMinutes(90);
        public string Ubicacion { get; private set; }
        public int CantidadParticipantes { get; private set; }
        public List<Jugador.Jugador> Jugadores { get; private set; } = [];
        public Deporte.Deporte Deporte { get; private set; }
        public NivelHabilidad NivelMinimo { get; private set; } = NivelHabilidad.Basico;
        public NivelHabilidad NivelMaximo { get; private set; } = NivelHabilidad.Experto;
        public int PartidosMinimosJugados { get; private set; } = 0;
        public IEstrategiaEmparejamiento EstrategiaEmparejamiento { get; private set; }

        public static Partido Create(PartidoForCreation partidoForCreation)
        {
            return new Partido
            {
                FechaHora = partidoForCreation.FechaHora,
                Ubicacion = partidoForCreation.Ubicacion,
                CantidadParticipantes = partidoForCreation.CantidadParticipantes,
                Deporte = partidoForCreation.Deporte,
                NivelMinimo = partidoForCreation.NivelMinimo,
                NivelMaximo = partidoForCreation.NivelMaximo,
                PartidosMinimosJugados = partidoForCreation.PartidosMinimosJugados,
                EstrategiaEmparejamiento = partidoForCreation.EstrategiaEmparejamiento
            };
        }

        public void AvanzarEstado()
        {
            Estado.Siguiente(this);
        }

        public void Cancelar()
        {
            Estado.Cancelar(this);
        }

        public void CambiarEstado(IPartidoState nuevoEstado)
        {
            Estado = nuevoEstado;
            NotificarObservers();
        }

        private void NotificarObservers()
        {
            foreach (var obs in Jugadores)
            {
                obs.NotificarCambioEstado(Estado.Nombre, this);
            }
        }

        public bool TieneJugadoresSuficientes() => Jugadores.Count >= CantidadParticipantes;
        public bool JugadoresConfirmados() => Jugadores.All(j => j.Confirmado);
    }
}
