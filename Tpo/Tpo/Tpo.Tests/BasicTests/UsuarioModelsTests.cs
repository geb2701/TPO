using Xunit;
using FluentAssertions;
using Tpo.Domain.Usuario.Models;
using Tpo.Domain;

namespace Tpo.Tests.BasicTests;

public class UsuarioModelsTests
{
    [Fact]
    public void UsuarioForCreation_PuedeCrearseConTodasLasPropiedades()
    {
        // Arrange & Act
        var model = new UsuarioForCreation
        {
            Nombre = "Juan Pérez",
            Alias = "juanp",
            Email = "juan@example.com",
            Contrasena = "password123",
            Ubicacion = "Buenos Aires",
            TipoNotificacion = TipoNotificacion.Email
        };

        // Assert
        model.Should().NotBeNull();
        model.Nombre.Should().Be("Juan Pérez");
        model.Alias.Should().Be("juanp");
        model.Email.Should().Be("juan@example.com");
        model.Contrasena.Should().Be("password123");
        model.Ubicacion.Should().Be("Buenos Aires");
        model.TipoNotificacion.Should().Be(TipoNotificacion.Email);
    }

    [Fact]
    public void UsuarioForUpdate_PuedeCrearseConTodasLasPropiedades()
    {
        // Arrange & Act
        var model = new UsuarioForUpdate
        {
            Nombre = "María García",
            Ubicacion = "Córdoba",
            TipoNotificacion = TipoNotificacion.PushCelular
        };

        // Assert
        model.Should().NotBeNull();
        model.Nombre.Should().Be("María García");
        model.Ubicacion.Should().Be("Córdoba");
        model.TipoNotificacion.Should().Be(TipoNotificacion.PushCelular);
    }

    [Theory]
    [InlineData(TipoNotificacion.Ninguna)]
    [InlineData(TipoNotificacion.Email)]
    [InlineData(TipoNotificacion.PushCelular)]
    [InlineData(TipoNotificacion.InApp)]
    [InlineData(TipoNotificacion.EmailYPush)]
    [InlineData(TipoNotificacion.Todas)]
    public void UsuarioForCreation_AceptaTodosLosTiposNotificacion(TipoNotificacion tipo)
    {
        // Arrange & Act
        var model = new UsuarioForCreation
        {
            TipoNotificacion = tipo
        };

        // Assert
        model.TipoNotificacion.Should().Be(tipo);
    }

    [Theory]
    [InlineData("a", "a@b.c")]
    [InlineData("usuario123", "usuario@domain.com")]
    [InlineData("user_name-test", "test.user@example.org")]
    public void UsuarioForCreation_AceptaDiferentesAliasYEmails(string alias, string email)
    {
        // Arrange & Act
        var model = new UsuarioForCreation
        {
            Alias = alias,
            Email = email
        };

        // Assert
        model.Alias.Should().Be(alias);
        model.Email.Should().Be(email);
    }

    [Fact]
    public void UsuarioForCreation_TieneTodasLasPropiedadesEsperadas()
    {
        // Arrange
        var type = typeof(UsuarioForCreation);

        // Act & Assert
        type.GetProperty("Nombre").Should().NotBeNull();
        type.GetProperty("Alias").Should().NotBeNull();
        type.GetProperty("Email").Should().NotBeNull();
        type.GetProperty("Contrasena").Should().NotBeNull();
        type.GetProperty("Ubicacion").Should().NotBeNull();
        type.GetProperty("TipoNotificacion").Should().NotBeNull();
    }

    [Fact]
    public void UsuarioForUpdate_TieneTodasLasPropiedadesEsperadas()
    {
        // Arrange
        var type = typeof(UsuarioForUpdate);

        // Act & Assert
        type.GetProperty("Nombre").Should().NotBeNull();
        type.GetProperty("Ubicacion").Should().NotBeNull();
        type.GetProperty("TipoNotificacion").Should().NotBeNull();
    }
} 