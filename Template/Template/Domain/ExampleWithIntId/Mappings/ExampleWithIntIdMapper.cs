using Riok.Mapperly.Abstractions;
using Template.Domain.ExampleWithIntId.Dtos;
using Template.Domain.ExampleWithIntId.Models;

namespace Template.Domain.ExampleWithIntId.Mappings;

[Mapper]
[UseStaticMapper(typeof(ExampleWithIntIdMapper))]
public static partial class ExampleWithIntIdMapper
{
    public static partial ExampleWithIntIdForCreation ToExampleWithIntIdForCreation(
        this ExampleWithIntIdForCreationDto dto);

    public static partial ExampleWithIntIdForUpdate ToExampleWithIntIdForUpdate(
        this ExampleWithIntIdForUpdateDto dto);

    public static partial ExampleWithIntIdDto ToExampleWithIntIdDto(this ExampleWithIntId entity);

    public static partial IQueryable<ExampleWithIntIdDto>
        ToExampleWithIntIdDtoQueryable(this IQueryable<ExampleWithIntId> entity);
}