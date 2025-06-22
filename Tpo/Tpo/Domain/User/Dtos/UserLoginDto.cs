namespace Tpo.Domain.User.Dtos
{
    public sealed record UserLoginDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}