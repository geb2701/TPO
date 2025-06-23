using Riok.Mapperly.Abstractions;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Models;

namespace Tpo.Domain.Partido.Mappings;

[Mapper]
[UseStaticMapper(typeof(PartidoMapper))]
public static partial class PartidoMapper
{
    public static partial PartidoForCreation ToPartidoForCreation(
        this PartidoForCreationDto dto);

    public static partial PartidoForUpdate ToPartidoForUpdate(
        this PartidoForUpdateDto dto);

    public static partial PartidoDto ToPartidoDto(this Partido entity);

    public static partial IQueryable<PartidoDto>
        ToPartidoDtoQueryable(this IQueryable<Partido> entity);
}