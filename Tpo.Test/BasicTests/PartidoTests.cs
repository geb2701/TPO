using FluentAssertions;
using Tpo.Domain;
using Tpo.Domain.Partido.Models;
using Xunit;

namespace Tpo.Test.BasicTests;

public class PartidoTests
{
    [Fact]
    public void PartidoForCreation_PuedeCrearseConPropiedadesBasicas()
    {
        // Arrange & Act
        var partidoCreation = new PartidoForCreation
        {
            FechaHora = DateTime.Now.AddDays(1),
            Ubicacion = "Cancha de fútbol",
            CantidadParticipantes = 10,
            Duracion = TimeSpan.FromMinutes(90)
        };

        // Assert
        partidoCreation.Should().NotBeNull();
        partidoCreation.Ubicacion.Should().Be("Cancha de fútbol");
        partidoCreation.CantidadParticipantes.Should().Be(10);
        partidoCreation.Duracion.Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void PartidoForCreation_TieneValoresPorDefecto()
    {
        // Arrange & Act
        var partidoCreation = new PartidoForCreation();

        // Assert
        partidoCreation.Duracion.Should().Be(TimeSpan.FromMinutes(90));
        partidoCreation.NivelMinimo.Should().Be(NivelHabilidad.Principiante);
        partidoCreation.NivelMaximo.Should().Be(NivelHabilidad.Experto);
        partidoCreation.PartidosMinimosJugados.Should().Be(0);
    }

    [Theory]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(120)]
    public void PartidoForCreation_AceptaDiferentesDuraciones(int minutos)
    {
        // Arrange & Act
        var partidoCreation = new PartidoForCreation
        {
            Duracion = TimeSpan.FromMinutes(minutos)
        };

        // Assert
        partidoCreation.Duracion.Should().Be(TimeSpan.FromMinutes(minutos));
    }

    [Theory]
    [InlineData(NivelHabilidad.Principiante, NivelHabilidad.Intermedio)]
    [InlineData(NivelHabilidad.Intermedio, NivelHabilidad.Avanzado)]
    [InlineData(NivelHabilidad.Avanzado, NivelHabilidad.Experto)]
    [InlineData(NivelHabilidad.Principiante, NivelHabilidad.Experto)]
    public void PartidoForCreation_AceptaDiferentesNiveles(NivelHabilidad minimo, NivelHabilidad maximo)
    {
        // Arrange & Act
        var partidoCreation = new PartidoForCreation
        {
            NivelMinimo = minimo,
            NivelMaximo = maximo
        };

        // Assert
        partidoCreation.NivelMinimo.Should().Be(minimo);
        partidoCreation.NivelMaximo.Should().Be(maximo);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(11)]
    [InlineData(22)]
    public void PartidoForCreation_AceptaDiferentesCantidadParticipantes(int cantidad)
    {
        // Arrange & Act
        var partidoCreation = new PartidoForCreation
        {
            CantidadParticipantes = cantidad
        };

        // Assert
        partidoCreation.CantidadParticipantes.Should().Be(cantidad);
    }

    [Fact]
    public void PartidoForCreation_PuedeEstablecerFechaFutura()
    {
        // Arrange
        var fechaFutura = DateTime.Now.AddDays(7).AddHours(3);

        // Act
        var partidoCreation = new PartidoForCreation
        {
            FechaHora = fechaFutura
        };

        // Assert
        partidoCreation.FechaHora.Should().Be(fechaFutura);
    }

    [Theory]
    [InlineData("Estadio Municipal")]
    [InlineData("Club Deportivo Central")]
    [InlineData("Polideportivo Norte")]
    public void PartidoForCreation_AceptaDiferentesUbicaciones(string ubicacion)
    {
        // Arrange & Act
        var partidoCreation = new PartidoForCreation
        {
            Ubicacion = ubicacion
        };

        // Assert
        partidoCreation.Ubicacion.Should().Be(ubicacion);
    }
}