namespace Tpo.Domain.Partido.Models;

public sealed record PartidoForCreation
{
    public DateTime FechaHora { get; set; }
    public string Ubicacion { get; set; }
    public int CantidadParticipantes { get; set; }
    public TimeSpan Duracion { get; set; } = TimeSpan.FromMinutes(90);
    public Deporte.Deporte Deporte { get; set; }
    public NivelHabilidad NivelMinimo { get; set; } = NivelHabilidad.Basico;
    public NivelHabilidad NivelMaximo { get; set; } = NivelHabilidad.Experto;
    public int PartidosMinimosJugados { get; set; } = 0;
    public IEstrategiaEmparejamiento EstrategiaEmparejamiento { get; set; }
}