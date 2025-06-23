namespace Tpo.Domain.Deporte.Dtos;

public sealed record DeporteDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    // Atributos de la clase deporte
    public string Nombre { get; set; }
}