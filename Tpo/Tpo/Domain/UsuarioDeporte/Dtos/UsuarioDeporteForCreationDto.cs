using System.ComponentModel.DataAnnotations;

namespace Tpo.Domain.UsuarioDeporte.Dtos
{
    public class UsuarioDeporteForCreationDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int DeporteId { get; set; }

        [Required]
        [Range(1, 10)]
        public int Nivel { get; set; }
    }
}
