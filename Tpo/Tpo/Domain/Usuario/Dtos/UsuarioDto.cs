namespace Tpo.Domain.Usuario.Dtos;

public sealed record UsuarioDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public string Alias { get; set; }
    public string Nombre { get; set; }
    public string Contrasena { get; set; }
    public string Email { get; set; }
    public string Ubicacion { get; set; }
    public List<HabilidadDto> Habilidades { get; set; } = [];
    public EnumResponse TipoNotificacion { get; set; }
    public List<ParticipanteDto> Participante { get; set; }
}

public sealed record HabilidadDto
{
    public int DeporteId { get; set; }
    public string DeporteNombre { get; set; }
    public EnumResponse Nivel { get; set; }
}

public sealed record ParticipanteDto
{
    public int DeporteId { get; set; }
    public string DeporteNombre { get; set; }
    public int PartidoId { get; set; }
    public string PartidoEstado { get; set; }
}