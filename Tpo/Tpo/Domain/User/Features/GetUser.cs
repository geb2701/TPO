using MediatR;
using Tpo.Domain.User.Mappings;
using Tpo.Domain.User.Dtos;
using Tpo.Domain.User.Services;

namespace Tpo.Domain.User.Features;

public static class GetUser
{
    public sealed record Query(int Id) : IRequest<UserDto>;

    public sealed class Handler : IRequestHandler<Query, UserDto>
    {
        private readonly IUserRepository _repository;

        public Handler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity =
                await _repository.GetById(request.Id, cancellationToken);

            return entity.ToUserDto();
        }
    }
}