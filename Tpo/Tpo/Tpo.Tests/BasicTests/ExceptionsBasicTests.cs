using Xunit;
using FluentAssertions;
using Tpo.Exceptions;
using System;

namespace Tpo.Tests.BasicTests;

public class ExceptionsBasicTests
{
    [Fact]
    public void NotFoundException_PuedeCrearseSinParametros()
    {
        // Arrange & Act
        var exception = new NotFoundException();

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<NotFoundException>();
        exception.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void NotFoundException_PuedeCrearseConMensaje()
    {
        // Arrange
        var mensaje = "El recurso no fue encontrado";

        // Act
        var exception = new NotFoundException(mensaje);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(mensaje);
    }

    [Fact]
    public void NotFoundException_PuedeCrearseConMensajeYExcepcionInterna()
    {
        // Arrange
        var mensaje = "Error al buscar";
        var innerException = new InvalidOperationException("Operación inválida");

        // Act
        var exception = new NotFoundException(mensaje, innerException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(mensaje);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void NotFoundException_HeredaDeException()
    {
        // Arrange & Act
        var exception = new NotFoundException();

        // Assert
        exception.Should().BeAssignableTo<Exception>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Error")]
    [InlineData("Un mensaje muy largo para probar que acepta cualquier longitud de mensaje")]
    public void NotFoundException_AceptaCualquierMensaje(string mensaje)
    {
        // Act
        var exception = new NotFoundException(mensaje);

        // Assert
        exception.Message.Should().Be(mensaje);
    }
} 