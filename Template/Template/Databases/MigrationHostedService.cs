using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net.Sockets;
using ILogger = Serilog.ILogger;

namespace Template.Databases;

public class MigrationHostedService<TDbContext>
    : IHostedService
    where TDbContext : DbContext
{
    private readonly ILogger _logger = Log.ForContext<MigrationHostedService<TDbContext>>();
    private readonly IServiceScopeFactory _scopeFactory;

    public MigrationHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information("Applying migrations for {DbContext}", typeof(TDbContext).Name);

            await using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await context.Database.MigrateAsync(cancellationToken);

            _logger.Information("Migrations complete for {DbContext}", typeof(TDbContext).Name);
        }
        catch (Exception ex) when (ex is SocketException)
        {
            _logger.Error(ex,
                "Could not connect to the database. Please check the connection string and make sure the database is running.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while applying the database migrations.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}