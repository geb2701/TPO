using Xunit;
using FluentAssertions;
using Tpo.Domain.Deporte.Models;

namespace Tpo.Tests.ExampleTests;

public class SimpleDeporteTests
{
    [Fact]
    public void Deporte_Create_ShouldCreateValidEntity()
    {
        // Arrange
        var deporteForCreation = new DeporteForCreation
        {
            Nombre = "Fútbol"
        };

        // Act
        var deporte = Tpo.Domain.Deporte.Deporte.Create(deporteForCreation);

        // Assert
        deporte.Should().NotBeNull();
        deporte.Nombre.Should().Be("Fútbol");
    }

    [Fact]
    public void Deporte_Update_ShouldUpdateName()
    {
        // Arrange
        var deporte = Tpo.Domain.Deporte.Deporte.Create(new DeporteForCreation { Nombre = "Tenis" });
        var updateModel = new DeporteForUpdate { Nombre = "Tenis de Mesa" };

        // Act
        deporte.Update(updateModel);

        // Assert
        deporte.Nombre.Should().Be("Tenis de Mesa");
    }

    [Theory]
    [InlineData("Fútbol")]
    [InlineData("Básquet")]
    [InlineData("Natación")]
    public void Deporte_Create_WithDifferentSports_ShouldWork(string nombreDeporte)
    {
        // Arrange & Act
        var deporte = Tpo.Domain.Deporte.Deporte.Create(new DeporteForCreation { Nombre = nombreDeporte });

        // Assert
        deporte.Should().NotBeNull();
        deporte.Nombre.Should().Be(nombreDeporte);
    }
} 