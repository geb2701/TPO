using FluentAssertions;
using Tpo.Domain.Deporte.Models;
using Xunit;

namespace Tpo.Test.Domain.Deporte;

public class DeporteDomainTests
{
    [Fact]
    public void Deporte_Create_CreaDeporteConNombre()
    {
        // Arrange
        var deporteCreation = new DeporteForCreation
        {
            Nombre = "Fútbol"
        };

        // Act
        var deporte = Tpo.Domain.Deporte.Deporte.Create(deporteCreation);

        // Assert
        deporte.Should().NotBeNull();
        deporte.Nombre.Should().Be("Fútbol");
        deporte.Id.Should().Be(0);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Básquetbol")]
    [InlineData("Tenis de Mesa")]
    [InlineData("Un deporte con un nombre muy largo para probar límites")]
    public void Deporte_Create_AceptaCualquierNombre(string nombre)
    {
        // Arrange
        var deporteCreation = new DeporteForCreation
        {
            Nombre = nombre
        };

        // Act
        var deporte = Tpo.Domain.Deporte.Deporte.Create(deporteCreation);

        // Assert
        deporte.Nombre.Should().Be(nombre);
    }

    [Fact]
    public void Deporte_Update_ActualizaNombre()
    {
        // Arrange
        var deporteCreation = new DeporteForCreation { Nombre = "Fútbol" };
        var deporte = Tpo.Domain.Deporte.Deporte.Create(deporteCreation);
        var deporteUpdate = new DeporteForUpdate { Nombre = "Básquetbol" };

        // Act
        var resultado = deporte.Update(deporteUpdate);

        // Assert
        resultado.Should().BeSameAs(deporte);
        deporte.Nombre.Should().Be("Básquetbol");
    }

    [Fact]
    public void Deporte_Update_RetornaLaMismaInstancia()
    {
        // Arrange
        var deporteCreation = new DeporteForCreation { Nombre = "Tenis" };
        var deporte = Tpo.Domain.Deporte.Deporte.Create(deporteCreation);
        var deporteUpdate = new DeporteForUpdate { Nombre = "Tenis de Mesa" };

        // Act
        var resultado = deporte.Update(deporteUpdate);

        // Assert
        resultado.Should().BeSameAs(deporte);
    }

    [Fact]
    public void Deporte_Create_UsuariosDeportesEsNullPorDefecto()
    {
        // Arrange
        var deporteCreation = new DeporteForCreation { Nombre = "Hockey" };

        // Act
        var deporte = Tpo.Domain.Deporte.Deporte.Create(deporteCreation);

        // Assert
        deporte.UsuariosDeportes.Should().BeNull();
    }
}