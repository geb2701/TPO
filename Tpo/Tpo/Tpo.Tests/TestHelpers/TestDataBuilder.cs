using Bogus;
using Tpo.Domain;
using Tpo.Domain.Deporte.Models;
using Tpo.Domain.Usuario.Models;
using Tpo.Domain.Partido.Models;
using Tpo.Domain.UsuarioDeporte.Models;
using Tpo.Domain.Partido;

namespace Tpo.Tests.TestHelpers;

public static class TestDataBuilder
{
    private static readonly Faker _faker = new Faker("es");

    public static Tpo.Domain.Deporte.Deporte CreateDeporte(string? nombre = null)
    {
        return Tpo.Domain.Deporte.Deporte.Create(new DeporteForCreation
        {
            Nombre = nombre ?? _faker.PickRandom("Fútbol", "Básquet", "Tenis", "Vóley", "Rugby", "Hockey", "Natación")
        });
    }

    public static Tpo.Domain.Usuario.Usuario CreateUsuario(
        string? alias = null,
        TipoNotificacion? tipoNotificacion = null,
        string? ubicacion = null)
    {
        var usuario = Tpo.Domain.Usuario.Usuario.Create(new UsuarioForCreation
        {
            Alias = alias ?? _faker.Internet.UserName(),
            Nombre = _faker.Person.FullName,
            Contrasena = _faker.Internet.Password(),
            Email = _faker.Internet.Email(),
            Ubicacion = ubicacion ?? _faker.Address.City(),
            TipoNotificacion = tipoNotificacion ?? _faker.PickRandom<TipoNotificacion>()
        });

        return usuario;
    }

    public static Tpo.Domain.Partido.Partido CreatePartido(
        Tpo.Domain.Deporte.Deporte? deporte = null,
        DateTime? fechaHora = null,
        string? ubicacion = null,
        int? cantidadParticipantes = null,
        NivelHabilidad? nivelMinimo = null,
        NivelHabilidad? nivelMaximo = null,
        IEstrategiaEmparejamiento? estrategia = null)
    {
        return Tpo.Domain.Partido.Partido.Create(new PartidoForCreation
        {
            FechaHora = fechaHora ?? DateTime.Now.AddDays(_faker.Random.Int(1, 30)),
            Ubicacion = ubicacion ?? _faker.Address.StreetAddress(),
            CantidadParticipantes = cantidadParticipantes ?? _faker.Random.Int(4, 22),
            Deporte = deporte ?? CreateDeporte(),
            NivelMinimo = nivelMinimo ?? NivelHabilidad.Principiante,
            NivelMaximo = nivelMaximo ?? NivelHabilidad.Experto,
            PartidosMinimosJugados = _faker.Random.Int(0, 5),
            EstrategiaEmparejamiento = estrategia ?? new Tpo.Domain.Partido.EmparejamientoPorNivel()
        });
    }

    public static Tpo.Domain.UsuarioDeporte.UsuarioDeporte CreateUsuarioDeporte(
        Tpo.Domain.Usuario.Usuario? usuario = null,
        Tpo.Domain.Deporte.Deporte? deporte = null,
        NivelHabilidad? nivel = null)
    {
        return Tpo.Domain.UsuarioDeporte.UsuarioDeporte.Create(new UsuarioDeporteForCreation
        {
            Usuario = usuario ?? CreateUsuario(),
            Deporte = deporte ?? CreateDeporte(),
            Nivel = nivel ?? _faker.PickRandom<NivelHabilidad>()
        });
    }

    public static void SetEntityId<T>(T entity, int id) where T : class
    {
        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty != null && idProperty.CanWrite)
        {
            idProperty.SetValue(entity, id);
        }
    }
} 