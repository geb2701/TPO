using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryKit;
using QueryKit.Configuration;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Mappings;
using Tpo.Domain.Partido.Services;
using Tpo.Resources.QueryKitUtilities;

namespace Tpo.Domain.Partido.Features;
public static class GetPartidoList
{
    public sealed record Query(PartidoParametersDto QueryParameters) : IRequest<PagedList<PartidoDto>>;

    public sealed class Handler(IPartidoRepository repository) : IRequestHandler<Query, PagedList<PartidoDto>>
    {
        public async Task<PagedList<PartidoDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = repository.Query(cancellationToken: cancellationToken, includes: x => x.Include(y => y.Jugadores).ThenInclude(y => y.Usuario).Include(y => y.Deporte));

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToPartidoDtoQueryable();

            return await PagedList<PartidoDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}