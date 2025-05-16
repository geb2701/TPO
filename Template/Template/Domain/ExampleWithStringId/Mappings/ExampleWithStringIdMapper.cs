using Riok.Mapperly.Abstractions;
using Template.Domain.ExampleWithStringId.Dtos;
using Template.Domain.ExampleWithStringId.Models;

namespace Template.Domain.ExampleWithStringId.Mappings;

[Mapper]
[UseStaticMapper(typeof(ExampleWithStringIdMapper))]
public static partial class ExampleWithStringIdMapper
{
    public static partial ExampleWithStringIdForCreation ToExampleWithStringIdForCreation(
        this ExampleWithStringIdForCreationDto dto);

    public static partial ExampleWithStringIdForUpdate ToExampleWithStringIdForUpdate(
        this ExampleWithStringIdForUpdateDto dto);

    public static partial ExampleWithStringIdDto ToExampleWithStringIdDto(this ExampleWithStringId entity);

    public static partial IQueryable<ExampleWithStringIdDto>
        ToExampleWithStringIdDtoQueryable(this IQueryable<ExampleWithStringId> entity);
}