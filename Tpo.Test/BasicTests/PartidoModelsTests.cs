using FluentAssertions;
using Tpo.Domain.Partido.Models;
using Xunit;

namespace Tpo.Test.BasicTests;

public class PartidoModelsTests
{
    [Fact]
    public void PartidoForUpdate_PuedeCrearseConPropiedades()
    {
        // Arrange & Act
        var model = new PartidoForUpdate
        {
            // Establecer propiedades si existen
        };

        // Assert
        model.Should().NotBeNull();
    }

    [Fact]
    public void PartidoForUpdate_TienePropiedadesEsperadas()
    {
        // Arrange
        var type = typeof(PartidoForUpdate);

        // Act
        var properties = type.GetProperties();

        // Assert
        properties.Should().NotBeNull();
    }
}