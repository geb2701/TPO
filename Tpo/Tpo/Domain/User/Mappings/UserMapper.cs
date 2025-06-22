using Riok.Mapperly.Abstractions;
using Tpo.Domain.User.Dtos;
using Tpo.Domain.User.Models;

namespace Tpo.Domain.User.Mappings;

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