namespace Tpo.Domain.Usuario.Models;

public sealed record UsuarioForCreation
{
    public string UsuarioNombre { get; set; }
    public string Nombre { get; set; }
    public string Contrasena { get; set; }
    public string Email { get; set; }
    public string Ubicacion { get; set; }
    public TipoNotificacion TipoNotificacion { get; set; }
}