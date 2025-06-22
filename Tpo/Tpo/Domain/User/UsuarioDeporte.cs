using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Tpo.Domain.User
{
    public class UsuarioDeporte : BaseEntity<int>
    {
        public int UserId { get; set; }  // FK al User

        [Required]
        //public DeporteEnumerado Deporte { get; set; }

        public Nivel Nivel { get; set; }
    }
}
