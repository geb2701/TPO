namespace Tpo.Domain.Deporte.Dtos;

public sealed record UsuarioDeporteDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    // Atributos de la clase UsuarioDeporte
    public string Nombre { get; set; }
}