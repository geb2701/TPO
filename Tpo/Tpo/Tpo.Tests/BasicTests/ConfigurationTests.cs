using Xunit;
using FluentAssertions;
using Tpo.Configurations;

namespace Tpo.Tests.BasicTests;

public class ConfigurationTests
{
    [Fact]
    public void ConnectionStringOptions_PuedeCrearseSinValores()
    {
        // Arrange & Act
        var options = new ConnectionStringOptions();

        // Assert
        options.Should().NotBeNull();
        options.Tpo.Should().BeEmpty();
    }

    [Fact]
    public void ConnectionStringOptions_PuedeEstablecerConnectionString()
    {
        // Arrange
        var connectionString = "Server=localhost;Database=TpoDb;User=sa;Password=123456";
        
        // Act
        var options = new ConnectionStringOptions
        {
            Tpo = connectionString
        };

        // Assert
        options.Tpo.Should().Be(connectionString);
    }

    [Fact]
    public void ConnectionStringOptions_ConstanteSectionNameEsCorrecta()
    {
        // Assert
        ConnectionStringOptions.SectionName.Should().Be("ConnectionStrings");
    }

    [Fact]
    public void ConnectionStringOptions_ConstanteTpoKeyEsCorrecta()
    {
        // Assert
        ConnectionStringOptions.TpoKey.Should().Be("Tpo");
    }

    [Theory]
    [InlineData("")]
    [InlineData("Server=.")]
    [InlineData("Data Source=localhost;Initial Catalog=TpoDb;Integrated Security=True")]
    public void ConnectionStringOptions_AceptaDiferentesConnectionStrings(string connectionString)
    {
        // Arrange & Act
        var options = new ConnectionStringOptions
        {
            Tpo = connectionString
        };

        // Assert
        options.Tpo.Should().Be(connectionString);
    }

    [Fact]
    public void ConnectionStringOptions_ValorPorDefectoEsStringVacio()
    {
        // Arrange & Act
        var options = new ConnectionStringOptions();

        // Assert
        options.Tpo.Should().NotBeNull();
        options.Tpo.Should().Be(string.Empty);
    }
} 