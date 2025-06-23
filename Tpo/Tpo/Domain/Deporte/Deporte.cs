using SharedKernel.Domain.Entity;
using Tpo.Domain.Deporte.Models;

namespace Tpo.Domain.Deporte
{
    public class Deporte : BaseEntity<int>
    {
        protected Deporte() { }
        public string Nombre { get; private set; } //Aca estamos definiendo un atributo para la clase deporte, donde cualquiera lo va a poder tomar pero nadie lo va a poder modificar
        public List<UsuarioDeporte.UsuarioDeporte> UsuariosDeportes { get; set; }
        public static Deporte Create(DeporteForCreation deporteForCreation)
        {
            return new Deporte
            {
                Nombre = deporteForCreation.Nombre
            };
        }

        public Deporte Update(DeporteForUpdate deporteForUpdate)
        {
            Nombre = deporteForUpdate.Nombre;
            return this;//retornamos la misma instancia de la clase deporte, ya que no queremos crear una nueva instancia, sino actualizar la existente
        }
    }
}
