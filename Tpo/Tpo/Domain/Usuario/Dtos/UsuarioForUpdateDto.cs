namespace Tpo.Domain.Usuario.Dtos
{
    public sealed record UsuarioForUpdateDto
    {
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public string Ubicacion { get; set; }
    }
}