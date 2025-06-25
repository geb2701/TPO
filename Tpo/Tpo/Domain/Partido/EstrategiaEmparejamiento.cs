
namespace Tpo.Domain.Partido
{
    public interface IEstrategiaEmparejamiento
    {
        string Nombre { get; }
        List<Usuario.Usuario> SeleccionarJugadoresValidos(Partido partido, IEnumerable<Usuario.Usuario> candidatos);
        List<Jugador.Jugador> SeleccionarEInscribirJugadores(Partido partido, IEnumerable<Usuario.Usuario> candidatos);
    }

    public class EmparejamientoPorNivel : IEstrategiaEmparejamiento
    {
        public string Nombre => "PorNivel";

        public List<Jugador.Jugador> SeleccionarEInscribirJugadores(Partido partido, IEnumerable<Usuario.Usuario> candidatos)
        {
            var candidatosValidos = SeleccionarJugadoresValidos(partido, candidatos);

            var random = new Random();
            var candidatosMezclados = candidatosValidos.OrderBy(x => random.Next()).ToList();
            partido.AgregarJugadores(candidatosMezclados);

            return partido.Jugadores;
        }

        public List<Usuario.Usuario> SeleccionarJugadoresValidos(Partido partido, IEnumerable<Usuario.Usuario> candidatos)
        {
            var usuariosYaAnotados = partido.Jugadores.Select(j => j.UsuarioId).ToHashSet();

            var candidatosPosibles = candidatos
                .Where(j =>
                {
                    if (usuariosYaAnotados.Contains(j.Id))
                        return false;

                    var habilidad = j.Habilidades.FirstOrDefault(x => x.Deporte.Id == partido.Id);
                    if (habilidad == null) return false;

                    if (j.TienePartidoEnHorario(partido.FechaHora, partido.Duracion))
                        return false;

                    if (habilidad.Nivel < partido.NivelMinimo || habilidad.Nivel > partido.NivelMaximo)
                        return false;

                    return true;
                }).ToList();

            return candidatosPosibles;
        }
    }

    public class EmparejamientoPorCercania : IEstrategiaEmparejamiento
    {
        public string Nombre => "PorCercania";
        public List<Jugador.Jugador> SeleccionarEInscribirJugadores(Partido partido, IEnumerable<Usuario.Usuario> candidatos)
        {
            var candidatosValidos = SeleccionarJugadoresValidos(partido, candidatos);

            var random = new Random();
            var candidatosMezclados = candidatosValidos.OrderBy(x => random.Next()).ToList();
            partido.AgregarJugadores(candidatosMezclados);

            return partido.Jugadores;
        }
        public List<Usuario.Usuario> SeleccionarJugadoresValidos(Partido partido, IEnumerable<Usuario.Usuario> candidatos)
        {
            var usuariosYaAnotados = partido.Jugadores.Select(j => j.UsuarioId).ToHashSet();

            var candidatosPosibles = candidatos
                .Where(j => j.Ubicacion == partido.Ubicacion && !usuariosYaAnotados.Contains(j.Id)).ToList();

            return candidatosPosibles;
        }
    }

    public class EmparejamientoPorHistorial : IEstrategiaEmparejamiento
    {
        public string Nombre => "PorHistorial";
        public List<Jugador.Jugador> SeleccionarEInscribirJugadores(Partido partido, IEnumerable<Usuario.Usuario> candidatos)
        {
            var candidatosValidos = SeleccionarJugadoresValidos(partido, candidatos);

            var random = new Random();
            var candidatosMezclados = candidatosValidos.OrderBy(x => random.Next()).ToList();
            partido.AgregarJugadores(candidatosMezclados);

            return partido.Jugadores;
        }
        public List<Usuario.Usuario> SeleccionarJugadoresValidos(Partido partido, IEnumerable<Usuario.Usuario> candidatos)
        {
            var usuariosYaAnotados = partido.Jugadores.Select(j => j.UsuarioId).ToHashSet();

            var candidatosPosibles = candidatos
                .Where(j =>
                {
                    if (usuariosYaAnotados.Contains(j.Id))
                        return false;

                    var cantidadPatidos = j.Participante.Where(x => x.Partido.Estado is FinalizadoState);
                    return cantidadPatidos.Count() >= partido.PartidosMinimosJugados;
                }).ToList();

            return candidatosPosibles;
        }
    }
}
