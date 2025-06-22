namespace Tpo.Domain.Usuario.Models;

public sealed record UsuarioForUpdate
{
    public string Nombre { get; set; }
    public string Password { get; set; }
    public string Ubicacion { get; set; }
}