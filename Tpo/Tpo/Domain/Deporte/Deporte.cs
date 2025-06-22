using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Tpo.Domain.Deporte
{
    public class Deporte : BaseEntity<int>
    {
        [Required]
        public string Nombre { get; set; }

        // Constructor vacío para EF
        protected Deporte() { }

        public Deporte(string nombre)
        {
            Nombre = nombre;
        }
    }
}
