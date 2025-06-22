namespace Tpo.Domain.User.Models;

public sealed record UserForUpdate
{
    public string Name { get; set; }
    public string Password { get; set; }
}