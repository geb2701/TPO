namespace Tpo.Domain.Partido
{
    public interface IPartidoState
    {
        void Siguiente(Partido partido);
        void Cancelar(Partido partido);
        string Nombre { get; }
    }

    public class NecesitamosJugadoresState : IPartidoState
    {
        public string Nombre => "Necesitamos jugadores";

        public void Siguiente(Partido partido)
        {
            if (partido.TieneJugadoresSuficientes())
                partido.CambiarEstado(new PartidoArmadoState());
        }

        public void Cancelar(Partido partido)
        {
            partido.CambiarEstado(new CanceladoState());
        }
    }

    public class PartidoArmadoState : IPartidoState
    {
        public string Nombre => "Partido armado";

        public void Siguiente(Partido partido)
        {
            if (partido.JugadoresConfirmados())
                partido.CambiarEstado(new ConfirmadoState());
        }

        public void Cancelar(Partido partido)
        {
            partido.CambiarEstado(new CanceladoState());
        }
    }

    public class ConfirmadoState : IPartidoState
    {
        public string Nombre => "Confirmado";

        public void Siguiente(Partido partido)
        {
            if (DateTime.Now >= partido.FechaHora)
                partido.CambiarEstado(new EnJuegoState());
        }

        public void Cancelar(Partido partido)
        {
            partido.CambiarEstado(new CanceladoState());
        }
    }

    public class EnJuegoState : IPartidoState
    {
        public string Nombre => "En juego";

        public void Siguiente(Partido partido)
        {
            if (DateTime.Now >= partido.FechaHora.Add(partido.Duracion))
                partido.CambiarEstado(new FinalizadoState());
        }

        public void Cancelar(Partido partido)
        {
            partido.CambiarEstado(new CanceladoState());
        }
    }

    public class FinalizadoState : IPartidoState
    {
        public string Nombre => "Finalizado";

        public void Siguiente(Partido partido)
        {
            throw new InvalidOperationException("El partido ya ha finalizado y no puede avanzar a otro estado.");
        }

        public void Cancelar(Partido partido)
        {
            throw new InvalidOperationException("No se puede cancelar un partido que ya ha finalizado.");
        }
    }

    public class CanceladoState : IPartidoState
    {
        public string Nombre => "Cancelado";

        public void Siguiente(Partido partido)
        {
            throw new InvalidOperationException("El partido ha sido cancelado y no puede avanzar a otro estado.");
        }

        public void Cancelar(Partido partido)
        {
            throw new InvalidOperationException("El partido ya ha sido cancelado y no puede ser cancelado nuevamente.");
        }
    }

}
