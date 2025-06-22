namespace Tpo.Domain.Usuario.Dtos;

public sealed record UsuarioDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public string UsuarioNombre { get; set; }
    public string Nombre { get; set; }
    public string Contrasena { get; set; }
    public string Email { get; set; }
    public string Ubicacion { get; set; }
}