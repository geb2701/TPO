using Xunit;
using FluentAssertions;
using Tpo.Domain;

namespace Tpo.Tests.BasicTests;

public class EnumsTests
{
    [Theory]
    [InlineData(NivelHabilidad.Principiante, 1)]
    [InlineData(NivelHabilidad.Basico, 2)]
    [InlineData(NivelHabilidad.Intermedio, 3)]
    [InlineData(NivelHabilidad.Avanzado, 4)]
    [InlineData(NivelHabilidad.Experto, 5)]
    public void NivelHabilidad_TieneValoresCorrectos(NivelHabilidad nivel, int valorEsperado)
    {
        // Assert
        ((int)nivel).Should().Be(valorEsperado);
    }

    [Theory]
    [InlineData(TipoNotificacion.Ninguna, 0)]
    [InlineData(TipoNotificacion.Email, 1)]
    [InlineData(TipoNotificacion.PushCelular, 2)]
    [InlineData(TipoNotificacion.InApp, 3)]
    [InlineData(TipoNotificacion.EmailYPush, 4)]
    [InlineData(TipoNotificacion.Todas, 5)]
    public void TipoNotificacion_TieneValoresCorrectos(TipoNotificacion tipo, int valorEsperado)
    {
        // Assert
        ((int)tipo).Should().Be(valorEsperado);
    }

    [Fact]
    public void NivelHabilidad_TieneCincoNiveles()
    {
        // Assert
        var niveles = Enum.GetValues<NivelHabilidad>();
        niveles.Should().HaveCount(5);
    }

    [Fact]
    public void TipoNotificacion_TieneSeisTipos()
    {
        // Assert
        var tipos = Enum.GetValues<TipoNotificacion>();
        tipos.Should().HaveCount(6);
    }

    [Theory]
    [InlineData(1, NivelHabilidad.Principiante)]
    [InlineData(2, NivelHabilidad.Basico)]
    [InlineData(3, NivelHabilidad.Intermedio)]
    [InlineData(4, NivelHabilidad.Avanzado)]
    [InlineData(5, NivelHabilidad.Experto)]
    public void NivelHabilidad_CastDesdeInt(int valor, NivelHabilidad nivelEsperado)
    {
        // Act
        var nivel = (NivelHabilidad)valor;

        // Assert
        nivel.Should().Be(nivelEsperado);
    }

    [Theory]
    [InlineData(0, TipoNotificacion.Ninguna)]
    [InlineData(1, TipoNotificacion.Email)]
    [InlineData(2, TipoNotificacion.PushCelular)]
    [InlineData(3, TipoNotificacion.InApp)]
    [InlineData(4, TipoNotificacion.EmailYPush)]
    [InlineData(5, TipoNotificacion.Todas)]
    public void TipoNotificacion_CastDesdeInt(int valor, TipoNotificacion tipoEsperado)
    {
        // Act
        var tipo = (TipoNotificacion)valor;

        // Assert
        tipo.Should().Be(tipoEsperado);
    }

    [Fact]
    public void NivelHabilidad_TodosLosValoresSonUnicos()
    {
        // Arrange
        var valores = Enum.GetValues<NivelHabilidad>()
            .Select(n => (int)n)
            .ToList();

        // Assert
        valores.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void TipoNotificacion_TodosLosValoresSonUnicos()
    {
        // Arrange
        var valores = Enum.GetValues<TipoNotificacion>()
            .Select(t => (int)t)
            .ToList();

        // Assert
        valores.Should().OnlyHaveUniqueItems();
    }
} 