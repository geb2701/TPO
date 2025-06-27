using Xunit;
using FluentAssertions;
using Tpo.Domain.Usuario.Models;
using Tpo.Domain;

namespace Tpo.Tests.BasicTests;

public class UsuarioTests
{
    [Fact]
    public void UsuarioForCreation_PuedeCrearseConTodasLasPropiedades()
    {
        // Arrange & Act
        var usuarioCreation = new UsuarioForCreation
        {
            Alias = "jugador123",
            Nombre = "Juan Pérez",
            Contrasena = "password123",
            Email = "juan@test.com",
            Ubicacion = "Buenos Aires",
            TipoNotificacion = TipoNotificacion.Email
        };

        // Assert
        usuarioCreation.Should().NotBeNull();
        usuarioCreation.Alias.Should().Be("jugador123");
        usuarioCreation.Nombre.Should().Be("Juan Pérez");
        usuarioCreation.Contrasena.Should().Be("password123");
        usuarioCreation.Email.Should().Be("juan@test.com");
        usuarioCreation.Ubicacion.Should().Be("Buenos Aires");
        usuarioCreation.TipoNotificacion.Should().Be(TipoNotificacion.Email);
    }

    [Theory]
    [InlineData(TipoNotificacion.Ninguna)]
    [InlineData(TipoNotificacion.Email)]
    [InlineData(TipoNotificacion.PushCelular)]
    [InlineData(TipoNotificacion.InApp)]
    [InlineData(TipoNotificacion.EmailYPush)]
    [InlineData(TipoNotificacion.Todas)]
    public void UsuarioForCreation_AceptaDiferentesTiposNotificacion(TipoNotificacion tipo)
    {
        // Arrange & Act
        var usuarioCreation = new UsuarioForCreation
        {
            Alias = "test",
            Nombre = "Test",
            Contrasena = "pass",
            Email = "test@test.com",
            Ubicacion = "Test",
            TipoNotificacion = tipo
        };

        // Assert
        usuarioCreation.TipoNotificacion.Should().Be(tipo);
    }

    [Fact]
    public void UsuarioForUpdate_PuedeCrearseConPropiedades()
    {
        // Arrange & Act
        var usuarioUpdate = new UsuarioForUpdate
        {
            Nombre = "Nuevo Nombre"
        };

        // Assert
        usuarioUpdate.Should().NotBeNull();
        usuarioUpdate.Nombre.Should().Be("Nuevo Nombre");
    }

    [Theory]
    [InlineData("user1", "Usuario Uno", "user1@test.com")]
    [InlineData("admin", "Administrador", "admin@empresa.com")]
    [InlineData("player", "Jugador Pro", "player@deporte.com")]
    public void UsuarioForCreation_AceptaDiferentesUsuarios(string alias, string nombre, string email)
    {
        // Arrange & Act
        var usuarioCreation = new UsuarioForCreation
        {
            Alias = alias,
            Nombre = nombre,
            Contrasena = "defaultPass",
            Email = email,
            Ubicacion = "Default",
            TipoNotificacion = TipoNotificacion.Email
        };

        // Assert
        usuarioCreation.Alias.Should().Be(alias);
        usuarioCreation.Nombre.Should().Be(nombre);
        usuarioCreation.Email.Should().Be(email);
    }

    [Fact]
    public void UsuarioForCreation_PuedeCrearseConValoresMinimos()
    {
        // Arrange & Act
        var usuarioCreation = new UsuarioForCreation
        {
            Alias = "a",
            Nombre = "N",
            Contrasena = "p",
            Email = "e@e.c",
            Ubicacion = "L",
            TipoNotificacion = TipoNotificacion.Ninguna
        };

        // Assert
        usuarioCreation.Should().NotBeNull();
        usuarioCreation.Alias.Should().Be("a");
        usuarioCreation.Email.Should().Be("e@e.c");
    }
} 