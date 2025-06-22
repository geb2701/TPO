using SharedKernel.Domain.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tpo.Domain.User.Models;

namespace Tpo.Domain.User
{
    public class User : BaseEntity<int>
    {
        protected User(){}

        [Required] public string Name { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }

        [NotMapped] public List<UsuarioDeporte> Deportes { get; set; }

        public static User Create(UserForCreation userForCreation)
        {
            var newExample = new User
            {
                Name = userForCreation.Name,
                Password = userForCreation.Password,
                Email = userForCreation.Email
            };

            return newExample;
        }

        public User Update(UserForUpdate model)
        {
            Name = model.Name;
            Password = model.Password;
            return this;
        }
    }
}
