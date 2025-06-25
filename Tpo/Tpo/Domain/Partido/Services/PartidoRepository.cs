using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.Partido.Services;

public interface IPartidoRepository : IRepositoryIncludableQueryable<Partido, int>
{
}

public sealed class PartidoRepository(TpoDbContext dbContext) : GenericRepository<Partido, int>(dbContext), IPartidoRepository
{
    public override IQueryable<Partido> Query()
    {
        return base.Query();
    }

    public override async Task<Partido> GetByIdOrDefault(int id, bool withTracking = true, CancellationToken cancellationToken = default, params Func<IQueryable<Partido>, IIncludableQueryable<Partido, object>>[] includes)
    {
        var partido = await base.GetByIdOrDefault(id, withTracking, cancellationToken, includes);

        partido?.CambiarEstado(ReconstruirEstado(partido.EstadoNombre));

        return partido;
    }

    public override IQueryable<Partido> Query(
        Expression<Func<Partido, bool>> filter = null,
        bool withTracking = true, CancellationToken cancellationToken = default,
        params Func<IQueryable<Partido>, IIncludableQueryable<Partido, object>>[] includes)
    {
        var query = base.Query(filter, withTracking, cancellationToken, includes);

        return query.AsEnumerable().Select(p =>
        {
            p.CambiarEstado(ReconstruirEstado(p.EstadoNombre), false);
            return p;
        }).AsQueryable();
    }

    private static IPartidoState ReconstruirEstado(string nombre)
    {
        return nombre switch
        {
            "Necesitamos jugadores" => new NecesitamosJugadoresState(),
            "Partido armado" => new PartidoArmadoState(),
            "Confirmado" => new ConfirmadoState(),
            "En juego" => new EnJuegoState(),
            "Finalizado" => new FinalizadoState(),
            "Cancelado" => new CanceladoState(),
            _ => throw new InvalidOperationException($"Estado desconocido: {nombre}")
        };
    }
}