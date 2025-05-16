using MediatR;
using Template.Domain.ExampleWithIntId.Dtos;
using Template.Domain.ExampleWithIntId.Mappings;
using Template.Domain.ExampleWithIntId.Services;

namespace Template.Domain.ExampleWithIntId.Features;

public static class GetExampleWithIntId
{
    public sealed record Query(int Id) : IRequest<ExampleWithIntIdDto>;

    public sealed class Handler : IRequestHandler<Query, ExampleWithIntIdDto>
    {
        private readonly IExampleWithIntIdRepository _repository;

        public Handler(IExampleWithIntIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExampleWithIntIdDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity =
                await _repository.GetById(request.Id, cancellationToken);

            return entity.ToExampleWithIntIdDto();
        }
    }
}