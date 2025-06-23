using SharedKernel.Domain.Entity;
using Tpo.Domain.Partido.Models;

namespace Tpo.Domain.Partido
{
    public class Partido : BaseEntity<int>
    {
        protected Partido() { }
        public string Nombre { get; private set; } //Aca estamos definiendo un atributo para la clase Partido, donde cualquiera lo va a poder tomar pero nadie lo va a poder modificar
        public static Partido Create(PartidoForCreation PartidoForCreation)
        {
            return new Partido
            {
                Nombre = PartidoForCreation.Nombre
            };
        }

        public Partido Update(PartidoForUpdate partidoForUpdate)
        {
            Nombre = partidoForUpdate.Nombre;
            return this;//retornamos la misma instancia de la clase Partido, ya que no queremos crear una nueva instancia, sino actualizar la existente
        }
    }
}
