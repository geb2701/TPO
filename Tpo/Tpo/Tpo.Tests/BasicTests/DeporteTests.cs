using Xunit;
using FluentAssertions;
using Tpo.Domain.Deporte.Models;

namespace Tpo.Tests.BasicTests;

public class DeporteTests
{
    [Fact]
    public void DeporteForCreation_PuedeCrearseConNombre()
    {
        // Arrange & Act
        var deporteCreation = new DeporteForCreation
        {
            Nombre = "Fútbol"
        };

        // Assert
        deporteCreation.Should().NotBeNull();
        deporteCreation.Nombre.Should().Be("Fútbol");
    }

    [Fact]
    public void DeporteForUpdate_PuedeCrearseConNombre()
    {
        // Arrange & Act
        var deporteUpdate = new DeporteForUpdate
        {
            Nombre = "Básquetbol"
        };

        // Assert
        deporteUpdate.Should().NotBeNull();
        deporteUpdate.Nombre.Should().Be("Básquetbol");
    }

    [Theory]
    [InlineData("Tenis")]
    [InlineData("Natación")]
    [InlineData("Voleibol")]
    [InlineData("Hockey")]
    public void DeporteForCreation_AceptaDiferentesDeportes(string nombre)
    {
        // Arrange & Act
        var deporteCreation = new DeporteForCreation
        {
            Nombre = nombre
        };

        // Assert
        deporteCreation.Nombre.Should().Be(nombre);
    }

    [Fact]
    public void DeporteForCreation_PuedeCrearseConNombreVacio()
    {
        // Arrange & Act
        var deporteCreation = new DeporteForCreation
        {
            Nombre = ""
        };

        // Assert
        deporteCreation.Nombre.Should().BeEmpty();
    }

    [Fact]
    public void DeporteForCreation_PuedeCrearseConNombreNulo()
    {
        // Arrange & Act
        var deporteCreation = new DeporteForCreation
        {
            Nombre = null
        };

        // Assert
        deporteCreation.Nombre.Should().BeNull();
    }
} 