using SharedKernel.Domain.Entity;
using Tpo.Domain.UsuarioDeporte.Models;

namespace Tpo.Domain.UsuarioDeporte
{
    public class UsuarioDeporte : BaseEntity<int>
    {
        public Usuario.Usuario Usuario { get; set; }
        public Deporte.Deporte Deporte { get; set; }
        public NivelHabilidad Nivel { get; set; }
        public bool Favorito { get; set; } = false;

        protected UsuarioDeporte() { }

        public static UsuarioDeporte Create(UsuarioDeporteForCreation usuarioDeporteForCreation)
        {
            return new UsuarioDeporte
            {
                Usuario = usuarioDeporteForCreation.Usuario,
                Deporte = usuarioDeporteForCreation.Deporte,
                Nivel = usuarioDeporteForCreation.Nivel,
                Favorito = usuarioDeporteForCreation.Favorito
            };
        }

        public UsuarioDeporte Update(UsuarioDeporteForUpdate usuarioDeporteForUpdate)
        {
            Nivel = usuarioDeporteForUpdate.Nivel;
            Favorito = usuarioDeporteForUpdate.Favorito;
            return this;
        }
    }
}
