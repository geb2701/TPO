using MediatR;
using QueryKit;
using QueryKit.Configuration;
using Template.Domain.ExampleWithStringId.Dtos;
using Template.Domain.ExampleWithStringId.Mappings;
using Template.Domain.ExampleWithStringId.Services;
using Template.Resources.QueryKitUtilities;

namespace Template.Domain.ExampleWithStringId.Features;

public static class GetExampleWithStringIdList
{
    public sealed record Query(ExampleWithStringIdParametersDto QueryParameters) : IRequest<PagedList<ExampleWithStringIdDto>>;

    public sealed class Handler : IRequestHandler<Query, PagedList<ExampleWithStringIdDto>>
    {
        private readonly IExampleWithStringIdRepository _repository;

        public Handler(IExampleWithStringIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<ExampleWithStringIdDto>> Handle(Query request, CancellationToken cancellationToken)
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
            var dtoCollection = appliedCollection.ToExampleWithStringIdDtoQueryable();

            return await PagedList<ExampleWithStringIdDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}