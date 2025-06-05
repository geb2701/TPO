using Riok.Mapperly.Abstractions;
using Template.Domain.User.Dtos;
using Template.Domain.User.Models;

namespace Template.Domain.User.Mappings;

[Mapper]
[UseStaticMapper(typeof(UserMapper))]
public static partial class UserMapper
{
    public static partial UserForCreation ToUserForCreation(
        this UserForCreationDto dto);

    public static partial UserForUpdate ToUserForUpdate(
        this UserForUpdateDto dto);

    public static partial UserDto ToUserDto(this User entity);

    public static partial IQueryable<UserDto>
        ToUserDtoQueryable(this IQueryable<User> entity);
}