using SharedKernel.Domain.Entity;
using Tpo.Domain.Usuario.Models;

namespace Tpo.Domain.Usuario
{
    public class Usuario : BaseEntity<int>
    {
        protected Usuario() { }

        public new int Id { get; internal set; } = default; //Ignorar
        public string UsuarioNombre { get; private set; }
        public string Nombre { get; private set; }
        public string Contrasena { get; private set; }
        public string Email { get; private set; }
        public string Ubicacion { get; private set; }
        public List<UsuarioDeporte.UsuarioDeporte> Habilidades { get; private set; }
        public List<Partido.Partido> Partidos { get; private set; }
        public TipoNotificacion TipoNotificacion { get; private set; }

        public static Usuario Create(UsuarioForCreation userForCreation)
        {
            var newExample = new Usuario
            {
                TipoNotificacion = userForCreation.TipoNotificacion,
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
            TipoNotificacion = model.TipoNotificacion;
            return this;
        }
    }
}
