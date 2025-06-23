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

    public static partial PartidoForUpdate ToPartidoForUpdate(
        this PartidoForUpdateDto dto);

    public static partial PartidoDto ToPartidoDto(this Partido entity);

    public static partial IQueryable<PartidoDto>
        ToPartidoDtoQueryable(this IQueryable<Partido> entity);
    [UserMapping]
    private static TimeSpan IntToTimeSpanMinutes(int x) => TimeSpan.FromMinutes(x);
}