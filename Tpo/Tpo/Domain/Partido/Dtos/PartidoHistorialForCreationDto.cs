namespace Tpo.Domain.Partido.Dtos
{
    public sealed record PartidoHistorialForCreationDto
    {
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; }
        public int CantidadParticipantes { get; set; }
        public int DuracionMinutos { get; set; }
        public int DeporteId { get; set; }
        public NivelHabilidad NivelMinimo { get; set; } = NivelHabilidad.Basico;
        public NivelHabilidad NivelMaximo { get; set; } = NivelHabilidad.Experto;
        public int PartidosMinimosJugados { get; set; } = 0;
    }
}