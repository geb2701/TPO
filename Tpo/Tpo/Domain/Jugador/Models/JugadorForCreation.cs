namespace Tpo.Domain.Jugador.Models
{
    public sealed record JugadorForCreation
    {
        public Usuario.Usuario Usuario { get; set; }
        public Partido.Partido Partido { get; set; }
    }
}