namespace Tpo.Domain.UsuarioDeporte.Dtos
{
    public class UsuarioDeporteForCreationDto
    {
        public int DeporteId { get; set; }
        public NivelHabilidad Nivel { get; set; }
        public bool Favorito { get; set; }
    }
}
