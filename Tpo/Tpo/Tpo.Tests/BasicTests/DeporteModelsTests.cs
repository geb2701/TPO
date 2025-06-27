using Xunit;
using FluentAssertions;
using Tpo.Domain.Deporte.Models;

namespace Tpo.Tests.BasicTests;

public class DeporteModelsTests
{
    [Fact]
    public void DeporteForCreation_PuedeCrearseConNombre()
    {
        // Arrange & Act
        var model = new DeporteForCreation
        {
            Nombre = "Natación"
        };

        // Assert
        model.Should().NotBeNull();
        model.Nombre.Should().Be("Natación");
    }

    [Fact]
    public void DeporteForUpdate_PuedeCrearseConNombre()
    {
        // Arrange & Act
        var model = new DeporteForUpdate
        {
            Nombre = "Tenis"
        };

        // Assert
        model.Should().NotBeNull();
        model.Nombre.Should().Be("Tenis");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("A")]
    [InlineData("Deporte con nombre muy largo para probar límites")]
    [InlineData("Fútbol 5")]
    [InlineData("Básquet-Ball")]
    public void DeporteForCreation_AceptaDiferentesNombres(string nombre)
    {
        // Arrange & Act
        var model = new DeporteForCreation
        {
            Nombre = nombre
        };

        // Assert
        model.Nombre.Should().Be(nombre);
    }

    [Theory]
    [InlineData("Hockey")]
    [InlineData("Rugby")]
    [InlineData("Ciclismo")]
    [InlineData("Running")]
    public void DeporteForUpdate_AceptaDiferentesNombres(string nombre)
    {
        // Arrange & Act
        var model = new DeporteForUpdate
        {
            Nombre = nombre
        };

        // Assert
        model.Nombre.Should().Be(nombre);
    }

    [Fact]
    public void DeporteForCreation_TienePropiedadNombre()
    {
        // Arrange & Act
        var type = typeof(DeporteForCreation);
        var nombreProperty = type.GetProperty("Nombre");

        // Assert
        nombreProperty.Should().NotBeNull();
        nombreProperty.PropertyType.Should().Be(typeof(string));
    }

    [Fact]
    public void DeporteForUpdate_TienePropiedadNombre()
    {
        // Arrange & Act
        var type = typeof(DeporteForUpdate);
        var nombreProperty = type.GetProperty("Nombre");

        // Assert
        nombreProperty.Should().NotBeNull();
        nombreProperty.PropertyType.Should().Be(typeof(string));
    }
} 