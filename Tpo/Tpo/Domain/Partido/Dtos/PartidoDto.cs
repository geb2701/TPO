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
    public string Nombre { get; set; }
}