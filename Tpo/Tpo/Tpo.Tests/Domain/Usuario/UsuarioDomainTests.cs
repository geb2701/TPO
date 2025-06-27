using Xunit;
using FluentAssertions;
using Tpo.Domain.Usuario;
using Tpo.Domain.Usuario.Models;
using Tpo.Domain;
using System;

namespace Tpo.Tests.Domain.Usuario;

public class UsuarioDomainTests
{
    [Fact]
    public void Usuario_Create_CreaUsuarioConTodasLasPropiedades()
    {
        // Arrange
        var usuarioCreation = new UsuarioForCreation
        {
            Nombre = "Juan Pérez",
            Email = "juan@test.com",
            Alias = "juanp",
            Contrasena = "password123",
            Ubicacion = "Buenos Aires",
            TipoNotificacion = TipoNotificacion.Email
        };

        // Act
        var usuario = Tpo.Domain.Usuario.Usuario.Create(usuarioCreation);

        // Assert
        usuario.Should().NotBeNull();
        usuario.Nombre.Should().Be("Juan Pérez");
        usuario.Email.Should().Be("juan@test.com");
        usuario.Alias.Should().Be("juanp");
        usuario.Contrasena.Should().Be("password123");
        usuario.Ubicacion.Should().Be("Buenos Aires");
        usuario.TipoNotificacion.Should().Be(TipoNotificacion.Email);
        usuario.Id.Should().Be(0);
    }

    [Theory]
    [InlineData(TipoNotificacion.Ninguna)]
    [InlineData(TipoNotificacion.Email)]
    [InlineData(TipoNotificacion.PushCelular)]
    [InlineData(TipoNotificacion.InApp)]
    [InlineData(TipoNotificacion.EmailYPush)]
    [InlineData(TipoNotificacion.Todas)]
    public void Usuario_Create_AceptaTodosLosTiposNotificacion(TipoNotificacion tipo)
    {
        // Arrange
        var usuarioCreation = new UsuarioForCreation
        {
            Nombre = "Test",
            Email = "test@test.com",
            Alias = "test",
            Contrasena = "test123",
            Ubicacion = "Test",
            TipoNotificacion = tipo
        };

        // Act
        var usuario = Tpo.Domain.Usuario.Usuario.Create(usuarioCreation);

        // Assert
        usuario.TipoNotificacion.Should().Be(tipo);
    }

    [Fact]
    public void Usuario_Update_ActualizaPropiedades()
    {
        // Arrange
        var usuarioCreation = new UsuarioForCreation
        {
            Nombre = "Original",
            Email = "original@test.com",
            Alias = "original",
            Contrasena = "original123",
            Ubicacion = "Original",
            TipoNotificacion = TipoNotificacion.Email
        };
        var usuario = Tpo.Domain.Usuario.Usuario.Create(usuarioCreation);
        
        var usuarioUpdate = new UsuarioForUpdate
        {
            Nombre = "Actualizado",
            Contrasena = "nuevo123",
            Ubicacion = "Nueva Ubicación",
            TipoNotificacion = TipoNotificacion.Todas
        };

        // Act
        var resultado = usuario.Update(usuarioUpdate);

        // Assert
        resultado.Should().BeSameAs(usuario);
        usuario.Nombre.Should().Be("Actualizado");
        usuario.Contrasena.Should().Be("nuevo123");
        usuario.Ubicacion.Should().Be("Nueva Ubicación");
        usuario.TipoNotificacion.Should().Be(TipoNotificacion.Todas);
        usuario.Email.Should().Be("original@test.com"); // No se actualiza
        usuario.Alias.Should().Be("original"); // No se actualiza
    }

    [Fact]
    public void Usuario_Create_HabilidadesYParticipanteNullosPorDefecto()
    {
        // Arrange
        var usuarioCreation = new UsuarioForCreation
        {
            Nombre = "Test",
            Email = "test@test.com",
            Alias = "test",
            Contrasena = "test123",
            Ubicacion = "Test",
            TipoNotificacion = TipoNotificacion.Email
        };

        // Act
        var usuario = Tpo.Domain.Usuario.Usuario.Create(usuarioCreation);

        // Assert
        usuario.Habilidades.Should().BeNull();
        usuario.Participante.Should().BeNull();
    }

    [Fact]
    public void Usuario_TienePartidoEnHorario_RetornaFalseSiNoTieneParticipaciones()
    {
        // Arrange
        var usuarioCreation = new UsuarioForCreation
        {
            Nombre = "Test",
            Email = "test@test.com",
            Alias = "test",
            Contrasena = "test123",
            Ubicacion = "Test",
            TipoNotificacion = TipoNotificacion.Email
        };
        var usuario = Tpo.Domain.Usuario.Usuario.Create(usuarioCreation);
        var fecha = DateTime.Now;
        var duracion = TimeSpan.FromHours(2);

        // Act
        var resultado = usuario.TienePartidoEnHorario(fecha, duracion);

        // Assert
        resultado.Should().BeFalse();
    }
} 