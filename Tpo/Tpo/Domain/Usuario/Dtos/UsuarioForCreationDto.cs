namespace Tpo.Domain.Usuario.Dtos
{
    public sealed record UsuarioForCreationDto
    {
        public string UsuarioNombre { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public string Email { get; set; }
        public string Ubicacion { get; set; }
    }
}