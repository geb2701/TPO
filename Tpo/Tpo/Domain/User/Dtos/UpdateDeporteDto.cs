using System.ComponentModel.DataAnnotations;

namespace Tpo.Domain.Deporte.Dtos
{
    public class UpdateDeporteDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required]
        [Range(1, 10)]
        public int Dificultad { get; set; }
    }
}
