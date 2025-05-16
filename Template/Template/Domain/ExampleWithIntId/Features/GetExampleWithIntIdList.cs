using MediatR;
using QueryKit;
using QueryKit.Configuration;
using Template.Domain.ExampleWithIntId.Dtos;
using Template.Domain.ExampleWithIntId.Mappings;
using Template.Domain.ExampleWithIntId.Services;
using Template.Resources.QueryKitUtilities;

namespace Template.Domain.ExampleWithIntId.Features;

public static class GetExampleWithIntIdList
{
    public sealed record Query(ExampleWithIntIdParametersDto QueryParameters) : IRequest<PagedList<ExampleWithIntIdDto>>;

    public sealed class Handler : IRequestHandler<Query, PagedList<ExampleWithIntIdDto>>
    {
        private readonly IExampleWithIntIdRepository _repository;

        public Handler(IExampleWithIntIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<ExampleWithIntIdDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = _repository.Query();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToExampleWithIntIdDtoQueryable();

            return await PagedList<ExampleWithIntIdDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}