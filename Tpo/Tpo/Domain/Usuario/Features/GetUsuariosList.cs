using MediatR;
using QueryKit;
using QueryKit.Configuration;
using Tpo.Domain.Usuario.Dtos;
using Tpo.Domain.Usuario.Mappings;
using Tpo.Domain.Usuario.Services;
using Tpo.Resources.QueryKitUtilities;

namespace Tpo.Domain.Usuario.Features;
public static class GetUsuariosList
{
    public sealed record Query(UsuarioParametersDto QueryParameters) : IRequest<PagedList<UsuarioDto>>;

    public sealed class Handler(IUsuarioRepository repository) : IRequestHandler<Query, PagedList<UsuarioDto>>
    {
        public async Task<PagedList<UsuarioDto>> Handle(Query request, CancellationToken cancellationToken)
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
            var dtoCollection = appliedCollection.ToUsuarioDtoQueryable();

            return await PagedList<UsuarioDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}