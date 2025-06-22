namespace Tpo.Domain.Deporte.Models;

public sealed record DeporteForUpdate
{
    public string Name { get; set; }
    public string Password { get; set; }
}