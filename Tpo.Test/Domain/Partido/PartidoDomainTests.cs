using FluentAssertions;
using Moq;
using Tpo.Domain;
using Tpo.Domain.Partido;
using Tpo.Domain.Partido.Models;
using Xunit;

namespace Tpo.Test.Domain.Partido;

public class PartidoDomainTests
{
    [Fact]
    public void Partido_Create_CreaPartidoConPropiedadesBasicas()
    {
        // Arrange
        var deporteMock = new Mock<Tpo.Domain.Deporte.Deporte>();
        var estrategiaMock = new Mock<IEstrategiaEmparejamiento>();

        var partidoCreation = new PartidoForCreation
        {
            FechaHora = DateTime.Now.AddDays(7),
            Ubicacion = "Cancha Central",
            CantidadParticipantes = 10,
            Deporte = deporteMock.Object,
            NivelMinimo = NivelHabilidad.Basico,
            NivelMaximo = NivelHabilidad.Avanzado,
            PartidosMinimosJugados = 5,
            EstrategiaEmparejamiento = estrategiaMock.Object
        };

        // Act
        var partido = Tpo.Domain.Partido.Partido.Create(partidoCreation);

        // Assert
        partido.Should().NotBeNull();
        partido.FechaHora.Should().Be(partidoCreation.FechaHora);
        partido.Ubicacion.Should().Be("Cancha Central");
        partido.CantidadParticipantes.Should().Be(10);
        partido.NivelMinimo.Should().Be(NivelHabilidad.Basico);
        partido.NivelMaximo.Should().Be(NivelHabilidad.Avanzado);
        partido.PartidosMinimosJugados.Should().Be(5);
        partido.Id.Should().Be(0);
    }

    [Fact]
    public void Partido_Create_EstadoInicialEsNecesitamosJugadores()
    {
        // Arrange
        var deporteMock = new Mock<Tpo.Domain.Deporte.Deporte>();
        var estrategiaMock = new Mock<IEstrategiaEmparejamiento>();

        var partidoCreation = new PartidoForCreation
        {
            FechaHora = DateTime.Now,
            Ubicacion = "Test",
            CantidadParticipantes = 5,
            Deporte = deporteMock.Object,
            EstrategiaEmparejamiento = estrategiaMock.Object
        };

        // Act
        var partido = Tpo.Domain.Partido.Partido.Create(partidoCreation);

        // Assert
        partido.Estado.Should().BeOfType<NecesitamosJugadoresState>();
    }

    [Fact]
    public void Partido_Create_DuracionPorDefectoEs90Minutos()
    {
        // Arrange
        var deporteMock = new Mock<Tpo.Domain.Deporte.Deporte>();
        var estrategiaMock = new Mock<IEstrategiaEmparejamiento>();

        var partidoCreation = new PartidoForCreation
        {
            FechaHora = DateTime.Now,
            Ubicacion = "Test",
            CantidadParticipantes = 5,
            Deporte = deporteMock.Object,
            EstrategiaEmparejamiento = estrategiaMock.Object
        };

        // Act
        var partido = Tpo.Domain.Partido.Partido.Create(partidoCreation);

        // Assert
        partido.Duracion.Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void Partido_Create_JugadoresInicialmenteVacio()
    {
        // Arrange
        var deporteMock = new Mock<Tpo.Domain.Deporte.Deporte>();
        var estrategiaMock = new Mock<IEstrategiaEmparejamiento>();

        var partidoCreation = new PartidoForCreation
        {
            FechaHora = DateTime.Now,
            Ubicacion = "Test",
            CantidadParticipantes = 5,
            Deporte = deporteMock.Object,
            EstrategiaEmparejamiento = estrategiaMock.Object
        };

        // Act
        var partido = Tpo.Domain.Partido.Partido.Create(partidoCreation);

        // Assert
        partido.Jugadores.Should().NotBeNull();
        partido.Jugadores.Should().BeEmpty();
    }

    [Fact]
    public void Partido_TieneJugadoresSuficientes_RetornaFalseSiNoHayJugadores()
    {
        // Arrange
        var deporteMock = new Mock<Tpo.Domain.Deporte.Deporte>();
        var estrategiaMock = new Mock<IEstrategiaEmparejamiento>();

        var partidoCreation = new PartidoForCreation
        {
            FechaHora = DateTime.Now,
            Ubicacion = "Test",
            CantidadParticipantes = 5,
            Deporte = deporteMock.Object,
            EstrategiaEmparejamiento = estrategiaMock.Object
        };
        var partido = Tpo.Domain.Partido.Partido.Create(partidoCreation);

        // Act
        var resultado = partido.TieneJugadoresSuficientes();

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void Partido_JugadoresConfirmados_RetornaTrueSiNoHayJugadores()
    {
        // Arrange
        var deporteMock = new Mock<Tpo.Domain.Deporte.Deporte>();
        var estrategiaMock = new Mock<IEstrategiaEmparejamiento>();

        var partidoCreation = new PartidoForCreation
        {
            FechaHora = DateTime.Now,
            Ubicacion = "Test",
            CantidadParticipantes = 5,
            Deporte = deporteMock.Object,
            EstrategiaEmparejamiento = estrategiaMock.Object
        };
        var partido = Tpo.Domain.Partido.Partido.Create(partidoCreation);

        // Act
        var resultado = partido.JugadoresConfirmados();

        // Assert
        resultado.Should().BeTrue(); // Todos (cero) jugadores están confirmados
    }
}