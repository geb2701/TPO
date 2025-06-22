namespace Tpo.Domain.User.Models;

public sealed record UserForCreation
{
    public string Name { get; set; }
    public string Password { get; set; }
}