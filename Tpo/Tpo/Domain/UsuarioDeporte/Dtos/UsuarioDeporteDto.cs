namespace Tpo.Domain.UsuarioDeporte.Dtos;

public sealed record UsuarioDeporteDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    // Atributos de la clase UsuarioDeporte
    public int UsuarioId { get; set; }
    public string Alias { get; set; }
    public int DeporteId { get; set; }
    public string DeporteNombre { get; set; }
    public EnumResponse Nivel { get; set; }
    public bool Favorito { get; set; }
}