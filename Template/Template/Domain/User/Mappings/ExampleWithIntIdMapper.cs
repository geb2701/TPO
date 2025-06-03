using Riok.Mapperly.Abstractions;
using Template.Domain.ExampleWithIntId.Dtos;
using Template.Domain.ExampleWithIntId.Models;
using Template.Domain.User.Dtos;
using Template.Domain.User.Models;

namespace Template.Domain.ExampleWithIntId.Mappings;

[Mapper]
[UseStaticMapper(typeof(ExampleWithIntIdMapper))]
public static partial class ExampleWithIntIdMapper
{
    public static partial UserForCreation ToExampleWithIntIdForCreation(
        this UserForCreationDto dto);

    public static partial UserForUpdate ToExampleWithIntIdForUpdate(
        this UserForUpdateDto dto);

    public static partial UserDto ToExampleWithIntIdDto(this User entity);

    public static partial IQueryable<UserDto>
        ToExampleWithIntIdDtoQueryable(this IQueryable<User> entity);
}