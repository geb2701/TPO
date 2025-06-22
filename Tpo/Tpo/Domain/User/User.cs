using SharedKernel.Domain.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tpo.Domain.User.Models;

namespace Tpo.Domain.User
{
    public class User : BaseEntity<int>
    {
        protected User()
        {
            Deportes = new List<UsuarioDeporte>();
        }

        [Required] public string Name { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }

        public List<UsuarioDeporte> Deportes { get; set; }

        public static User Create(UserForCreation exampleWithStringIdForCreation)
        {
            var newExample = new User
            {
                Name = exampleWithStringIdForCreation.Name,
                Password = exampleWithStringIdForCreation.Password,
                Email = exampleWithStringIdForCreation.Email
                // Los deportes se agregan aparte
            };

            return newExample;
        }

        public User Update(UserForUpdate model)
        {
            return this;
        }
    }
}
