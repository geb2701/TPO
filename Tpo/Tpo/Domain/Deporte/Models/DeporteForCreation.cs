namespace Tpo.Domain.Deporte.Models;

public sealed record DeporteForCreation
{
    public string Name { get; set; }
    public string Password { get; set; }
}