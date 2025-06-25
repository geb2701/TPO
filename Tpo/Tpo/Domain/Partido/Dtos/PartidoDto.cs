namespace Tpo.Domain.Partido.Dtos;

public sealed record PartidoDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    // Atributos de la clase partido
    public string Estado { get; set; }
    public DateTime FechaHora { get; set; }
    public int Duracion { get; set; }
    public string Ubicacion { get; set; }
    public int CantidadParticipantes { get; set; }
    public EnumResponse NivelMinimo { get; set; }
    public EnumResponse NivelMaximo { get; set; }
    public int PartidosMinimosJugados { get; set; }
    public string EstrategiaEmparejamiento { get; set; }
    public DeporteSimpleDto Deporte { get; set; }
    public List<JugadorDto> Jugadores { get; set; } = [];
}

public sealed record JugadorDto
{
    public int UsuarioId { get; set; }
    public string Alias { get; set; }
}

public sealed record DeporteSimpleDto
{
    public int DeporteId { get; set; }
    public string Nombre { get; set; }
}