using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Tpo.Domain.Deporte
{
    public class Deporte : BaseEntity<int>
    {
        protected Deporte() { }
        public string Nombre { get; private set; } //Aca estamos definiendo un atributo para la clase deporte, donde cualquiera lo va a poder tomar pero nadie lo va a poder modificar
        public static Deporte Create(DeporteForCreation deporteForCreation)
        {
            return new Deporte
            {
                Nombre = deporteForCreation.Nombre,
                Dificultad = deporteForCreation.Dificultad
            };
        }
        {
            return new Deporte
            {
                Nombre = nombre,
                Dificultad = dificultad
            };
        }

        public Deporte Update(DeporteForUpdate deporteForUpdate)
        {
            Nombre = nombre;
            Dificultad = dificultad;
            return this;
        }
    }
}
