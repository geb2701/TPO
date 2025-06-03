using MediatR;
using Template.Domain.ExampleWithIntId.Mappings;
using Template.Domain.User.Dtos;
using Template.Domain.User.Services;

namespace Template.Domain.ExampleWithIntId.Features;

public static class GetExampleWithIntId
{
    public sealed record Query(int Id) : IRequest<UserDto>;

    public sealed class Handler : IRequestHandler<Query, UserDto>
    {
        private readonly IExampleWithIntIdRepository _repository;

        public Handler(IExampleWithIntIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity =
                await _repository.GetById(request.Id, cancellationToken);

            return entity.ToExampleWithIntIdDto();
        }
    }
}