using SharedKernel.Domain.Entity;
using Tpo.Domain.Usuario.Models;

namespace Tpo.Domain.Usuario
{
    public class Usuario : BaseEntity<int>
    {
        protected Usuario() { }
        public override int Id { get; protected set; }
        public string Alias { get; private set; }
        public string Nombre { get; private set; }
        public string Contrasena { get; private set; }
        public string Email { get; private set; }
        public string Ubicacion { get; private set; }
        public List<UsuarioDeporte.UsuarioDeporte> Habilidades { get; private set; }
        public List<Jugador.Jugador> Participante { get; private set; }
        public TipoNotificacion TipoNotificacion { get; private set; }

        public static Usuario Create(UsuarioForCreation userForCreation)
        {
            var newExample = new Usuario
            {
                TipoNotificacion = userForCreation.TipoNotificacion,
                Alias = userForCreation.Alias,
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

        public bool TienePartidoEnHorario(DateTime fechaInicio, TimeSpan duracion)
        {
            var fechaFin = fechaInicio + duracion;
            return Participante?.Any(j =>
            {
                var partido = j.Partido;
                if (partido == null)
                    return false;
                var otroInicio = partido.FechaHora;
                var otroFin = partido.FechaHora + partido.Duracion;
                return fechaInicio < otroFin && fechaFin > otroInicio;
            }) ?? false;
        }
    }
}
