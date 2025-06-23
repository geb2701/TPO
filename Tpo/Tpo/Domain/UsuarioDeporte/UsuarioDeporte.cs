using SharedKernel.Domain.Entity;

namespace Tpo.Domain.UsuarioDeporte
{
    public class UsuarioDeporte : BaseEntity<int>
    {
        public int UsuarioId { get; set; }
        public int DeporteId { get; set; }
        public int Nivel { get; set; }

        protected UsuarioDeporte() { }

        public static UsuarioDeporte Create(int usuarioId, int deporteId, int nivel)
        {
            return new UsuarioDeporte
            {
                UsuarioId = usuarioId,
                DeporteId = deporteId,
                Nivel = nivel
            };
        }

        public UsuarioDeporte Update(int deporteId, int nivel)
        {
            DeporteId = deporteId;
            Nivel = nivel;
            return this;
        }
    }
}
