using Tpo.Domain.Jugador.Models;

namespace Tpo.Domain.Partido
{
    public interface IEstrategiaEmparejamiento
    {
        List<Jugador.Jugador> ObtenerJugadoresValidos(Partido partido, List<Usuario.Usuario> candidatos);
    }

    public class EmparejamientoPorNivel : IEstrategiaEmparejamiento
    {
        public List<Jugador.Jugador> ObtenerJugadoresValidos(Partido partido, List<Usuario.Usuario> candidatos)
        {
            var candidatosPosibles = candidatos
                .Where(j =>
                {
                    var habilidad = j.Habilidades.FirstOrDefault(x => x.Deporte.Id == partido.Id);
                    if (habilidad == null) return false;

                    if (habilidad.Nivel < partido.NivelMinimo || habilidad.Nivel > partido.NivelMaximo)
                        return false;

                    return true;
                }).ToList();

            return [.. candidatosPosibles.Select(x => Jugador.Jugador.Create(new JugadorForCreation
            {
                Usuario = x,
                Partido = partido
            }))];
        }
    }

    public class EmparejamientoPorCercania : IEstrategiaEmparejamiento
    {
        public List<Jugador.Jugador> ObtenerJugadoresValidos(Partido partido, List<Usuario.Usuario> candidatos)
        {
            var candidatosPosibles = candidatos
                .Where(j => j.Ubicacion == partido.Ubicacion).ToList();

            return [.. candidatosPosibles.Select(x => Jugador.Jugador.Create(new JugadorForCreation
            {
                Usuario = x,
                Partido = partido
            }))];
        }
    }

    public class EmparejamientoPorHistorial : IEstrategiaEmparejamiento
    {
        public List<Jugador.Jugador> ObtenerJugadoresValidos(Partido partido, List<Usuario.Usuario> candidatos)
        {
            var candidatosPosibles = candidatos
                .Where(j =>
                {
                    var cantidadPatidos = j.Participante.Where(x => x.Partido.Estado is FinalizadoState);
                    return cantidadPatidos.Count() >= partido.PartidosMinimosJugados;
                }).ToList();

            return [.. candidatosPosibles.Select(x => Jugador.Jugador.Create(new JugadorForCreation
            {
                Usuario = x,
                Partido = partido
            }))];
        }
    }
}
