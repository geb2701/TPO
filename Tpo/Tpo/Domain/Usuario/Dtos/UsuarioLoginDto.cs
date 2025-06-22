namespace Tpo.Domain.Usuario.Dtos
{
    public sealed record UsuarioLoginDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}