using SharedKernel.Domain.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tpo.Domain.Usuario.Models;

namespace Tpo.Domain.Usuario
{
    public class Usuario : BaseEntity<int>
    {
        protected Usuario(){}

        public string UsuarioNombre { get; private set; }
        public string Nombre { get; private set; }
        public string Contrasena { get; private set; }
        public string Email { get; private set; }
        public string Ubicacion { get; private set; }

        //[NotMapped] public List<UsuarioDeporte> Deportes { get; set; }

        public static Usuario Create(UsuarioForCreation userForCreation)
        {
            var newExample = new Usuario
            {
                UsuarioNombre = userForCreation.UsuarioNombre,
                Nombre = userForCreation.Nombre,
                Contrasena = userForCreation.Contrasena,
                Email = userForCreation.Email,
                Ubicacion = userForCreation.Ubicacion
            };

            return newExample;
        }

        public Usuario Update(UsuarioForUpdate model)
        {
            Nombre = model.Nombre;
            Contrasena = model.Contrasena;
            Ubicacion = model.Ubicacion;
            return this;
        }
    }
}
