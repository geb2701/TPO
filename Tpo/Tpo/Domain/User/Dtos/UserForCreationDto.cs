namespace Tpo.Domain.User.Dtos
{
    public sealed record UserForCreationDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}