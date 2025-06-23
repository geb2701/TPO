namespace Tpo.Domain.UsuarioDeporte.Models
{
    public class UsuarioDeporteForCreation
    {
        public Usuario.Usuario Usuario { get; set; }
        public Deporte.Deporte Deporte { get; set; }
        public NivelHabilidad Nivel { get; set; }
        public bool Favorito { get; set; }
    }
}
