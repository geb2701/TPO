
using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using Tpo.Domain.User.Models;
namespace Tpo.Domain.User;

public class User : BaseEntity<int>
{
    // For EF + Mocking
    protected User()
    {
    }

    [Required] public string Name { get; set; }
    [Required] public string Password { get; set; }

    public static User Create(UserForCreation exampleWithStringIdForCreation)
    {
        var newExample = new User
        {
            Name = exampleWithStringIdForCreation.Name,
            Password = exampleWithStringIdForCreation.Password
        };

        return newExample;
    }

    public User Update(UserForUpdate model)
    {
        return this;
    }
}