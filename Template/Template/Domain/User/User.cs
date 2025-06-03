
using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using Template.Domain.ExampleWithIntId.Models;
using Template.Domain.User.Models;
namespace Template.Domain.User;

public class User : BaseEntity<int>
{
    // For EF + Mocking
    protected User()
    {
    }

    [Required] public string Name { get; set; }

    public static User Create(UserForCreation exampleWithStringIdForCreation)
    {
        var newExample = new User
        {
            Name = exampleWithStringIdForCreation.Name
        };

        return newExample;
    }

    public User Update(UserForUpdate model)
    {
        return this;
    }
}