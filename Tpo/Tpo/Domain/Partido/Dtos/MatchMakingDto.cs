namespace Tpo.Domain.Partido.Dtos
{
    public class MatchMakingDto
    {
        public string Mensaje { get; set; }
        public List<JugadorDto> Jugadores { get; set; } = [];
    }
}
