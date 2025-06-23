using MediatR;
using QueryKit;
using QueryKit.Configuration;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Mappings;
using Tpo.Domain.Deporte.Services;
using Tpo.Resources.QueryKitUtilities;

namespace Tpo.Domain.Deporte.Features;
public static class GetDeportesList
{
    public sealed record Query(DeporteParametersDto QueryParameters) : IRequest<PagedList<DeporteDto>>;

    public sealed class Handler(IDeporteRepository repository) : IRequestHandler<Query, PagedList<DeporteDto>>
    {
        public async Task<PagedList<DeporteDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = repository.Query();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToDeporteDtoQueryable();

            return await PagedList<DeporteDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}