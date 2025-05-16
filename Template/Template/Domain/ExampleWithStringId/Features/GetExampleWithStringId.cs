using MediatR;
using Template.Domain.ExampleWithStringId.Dtos;
using Template.Domain.ExampleWithStringId.Mappings;
using Template.Domain.ExampleWithStringId.Services;

namespace Template.Domain.ExampleWithStringId.Features;

public static class GetExampleWithStringId
{
    public sealed record Query(string Code) : IRequest<ExampleWithStringIdDto>;

    public sealed class Handler : IRequestHandler<Query, ExampleWithStringIdDto>
    {
        private readonly IExampleWithStringIdRepository _repository;

        public Handler(IExampleWithStringIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExampleWithStringIdDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity =
                await _repository.GetById(request.Code, cancellationToken);

            return entity.ToExampleWithStringIdDto();
        }
    }
}