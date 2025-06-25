using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using Tpo.Domain.Jugador;
using Tpo.Domain.Jugador.Models;
using Tpo.Domain.Partido.Models;

namespace Tpo.Domain.Partido
{
    public class Partido : BaseEntity<int>, IJugadorObserver
    {
        protected Partido() { }
        public string EstadoNombre { get; private set; }
        [NotMapped] public IPartidoState Estado { get; private set; } = new NecesitamosJugadoresState();
        public DateTime FechaHora { get; private set; }
        public TimeSpan Duracion { get; private set; } = TimeSpan.FromMinutes(90);
        public string Ubicacion { get; private set; }
        public int CantidadParticipantes { get; private set; }
        public List<Jugador.Jugador> Jugadores { get; private set; } = [];
        public Deporte.Deporte Deporte { get; private set; }
        public NivelHabilidad NivelMinimo { get; private set; } = NivelHabilidad.Principiante;
        public NivelHabilidad NivelMaximo { get; private set; } = NivelHabilidad.Experto;
        public int PartidosMinimosJugados { get; private set; } = 0;
        public IEstrategiaEmparejamiento EstrategiaEmparejamiento { get; private set; }

        public static Partido Create(PartidoForCreation partidoForCreation)
        {
            return new Partido
            {
                EstadoNombre = new NecesitamosJugadoresState().Nombre,
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
        public void CambiarEstado(IPartidoState nuevoEstado, bool notificar = true)
        {
            Estado = nuevoEstado;
            EstadoNombre = nuevoEstado.Nombre;
            if (notificar)
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
        public void AgregarJugador(Usuario.Usuario usuario)
        {
            if (Estado is not NecesitamosJugadoresState)
                throw new InvalidOperationException("No se pueden agregar jugadores en el estado actual del partido.");

            if (TieneJugadoresSuficientes())
                throw new InvalidOperationException("El partido ya tiene la cantidad máxima de participantes.");

            if (Jugadores.Any(j => j.Usuario.Id == usuario.Id))
                throw new InvalidOperationException("El usuario ya está registrado en el partido.");

            var habilidad = usuario.Habilidades
                .FirstOrDefault(h => h.Deporte.Id == Deporte.Id) ?? throw new InvalidOperationException("El usuario no tiene habilidades registradas para este deporte.");

            if (habilidad.Nivel < NivelMinimo || habilidad.Nivel > NivelMaximo)
                throw new InvalidOperationException("El nivel de habilidad del usuario no es apto para este partido.");

            int partidosJugados = usuario.Participante?.Count ?? 0;
            if (partidosJugados < PartidosMinimosJugados)
                throw new InvalidOperationException("El usuario no cumple con la cantidad mínima de partidos jugados.");

            if (usuario.TienePartidoEnHorario(FechaHora, Duracion))
                throw new InvalidOperationException("El usuario ya está participando en otro partido en el mismo horario.");

            Jugador.Jugador.Create(new JugadorForCreation
            {
                Usuario = usuario,
                Partido = this
            });
        }

        public void AgregarJugadores(IEnumerable<Usuario.Usuario> usuario)
        {
            foreach (var u in usuario)
            {
                if (!TieneJugadoresSuficientes())
                    AgregarJugador(u);
            }
        }

        public void OnJugadorConfirmado()
        {
            if (Estado is PartidoArmadoState && JugadoresConfirmados())
            {
                AvanzarEstado();
            }
        }

        public void OnJugadorAgregado(Jugador.Jugador jugador)
        {
            Jugadores.Add(jugador);
            if (Estado is NecesitamosJugadoresState && TieneJugadoresSuficientes())
            {
                AvanzarEstado();
            }
        }

        public void Emparejamiento(IEnumerable<Usuario.Usuario> candidatos)
        {
            EstrategiaEmparejamiento.SeleccionarEInscribirJugadores(this, candidatos);
        }
    }
}
