using Riok.Mapperly.Abstractions;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Models;

namespace Tpo.Domain.Partido.Mappings;

[Mapper]
[UseStaticMapper(typeof(PartidoMapper))]
public static partial class PartidoMapper
{
    [MapProperty(nameof(PartidoUbicacionForCreationDto.DuracionMinutos), nameof(PartidoForCreation.Duracion))]
    [MapperIgnoreTarget(nameof(PartidoForCreation.NivelMinimo))]
    [MapperIgnoreTarget(nameof(PartidoForCreation.NivelMaximo))]
    [MapperIgnoreTarget(nameof(PartidoForCreation.PartidosMinimosJugados))]
    [MapperIgnoreSource(nameof(PartidoUbicacionForCreationDto.DeporteId))]
    private static partial PartidoForCreation ToPartidoForCreationPartial(this PartidoUbicacionForCreationDto dto, Deporte.Deporte deporte, IEstrategiaEmparejamiento estrategiaEmparejamiento);
    public static PartidoForCreation ToPartidoForCreation(this PartidoUbicacionForCreationDto dto, Deporte.Deporte deporte)
    {
        return ToPartidoForCreationPartial(dto, deporte, new EmparejamientoPorHistorial());
    }


    [MapProperty(nameof(PartidoUbicacionForCreationDto.DuracionMinutos), nameof(PartidoForCreation.Duracion))]
    [MapperIgnoreTarget(nameof(PartidoForCreation.PartidosMinimosJugados))]
    [MapperIgnoreSource(nameof(PartidoUbicacionForCreationDto.DeporteId))]
    private static partial PartidoForCreation ToPartidoForCreationPartial(this PartidoNivelForCreationDto dto, Deporte.Deporte deporte, IEstrategiaEmparejamiento estrategiaEmparejamiento);
    public static PartidoForCreation ToPartidoForCreation(this PartidoNivelForCreationDto dto, Deporte.Deporte deporte)
    {
        return ToPartidoForCreationPartial(dto, deporte, new EmparejamientoPorNivel());
    }

    [MapProperty(nameof(PartidoUbicacionForCreationDto.DuracionMinutos), nameof(PartidoForCreation.Duracion))]
    [MapperIgnoreTarget(nameof(PartidoForCreation.NivelMinimo))]
    [MapperIgnoreTarget(nameof(PartidoForCreation.NivelMaximo))]
    [MapperIgnoreSource(nameof(PartidoUbicacionForCreationDto.DeporteId))]
    private static partial PartidoForCreation ToPartidoForCreationPartial(this PartidoHistorialForCreationDto dto, Deporte.Deporte deporte, IEstrategiaEmparejamiento estrategiaEmparejamiento);
    public static PartidoForCreation ToPartidoForCreation(this PartidoHistorialForCreationDto dto, Deporte.Deporte deporte)
    {
        return ToPartidoForCreationPartial(dto, deporte, new EmparejamientoPorHistorial());
    }
    public static partial PartidoDto ToPartidoDto(this Partido entity);

    public static partial IQueryable<PartidoDto> ToPartidoDtoQueryable(this IQueryable<Partido> entity);
    [UserMapping]
    private static TimeSpan IntToTimeSpanMinutes(int x) => TimeSpan.FromMinutes(x);

    [UserMapping]
    private static int TimeSpanMinutesToInt(TimeSpan x) => (int)x.TotalMinutes;

    [UserMapping]
    private static EnumResponse ToEnum(NivelHabilidad x) => x.ToEnumResponse();

    [UserMapping]
    public static JugadorDto ToJugadorDto(Jugador.Jugador x) => new()
    {
        Alias = x.Usuario.Alias,
        UsuarioId = x.Usuario.Id
    };

    [UserMapping]
    private static DeporteSimpleDto ToDeporteSimpleDto(Deporte.Deporte x) => new()
    {
        DeporteId = x.Id,
        Nombre = x.Nombre
    };

    [UserMapping]
    private static string ToString(IEstrategiaEmparejamiento estrategiaEmparejamiento) => estrategiaEmparejamiento.Nombre;

    [UserMapping]
    private static string ToString(IPartidoState estado) => estado.Nombre;
}