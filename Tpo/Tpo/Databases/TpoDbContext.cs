using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Tpo.Domain.Deporte;
using Tpo.Domain.Jugador;
using Tpo.Domain.Partido;
using Tpo.Domain.Usuario;
using Tpo.Domain.UsuarioDeporte;
using Tpo.Services;

namespace Tpo.Databases;

public sealed class TpoDbContext(DbContextOptions<TpoDbContext> options,
    ICurrentUsuarioService currentUsuarioService) : DbContext(options)
{
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Deporte> Deporte { get; set; }
    public DbSet<UsuarioDeporte> UsuarioDeporte { get; set; }
    public DbSet<Partido> Partido { get; set; }
    public DbSet<Jugador> Jugador { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TpoDbContext).Assembly);
        modelBuilder.FilterSoftDeletedRecords();
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        var result = base.SaveChanges();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFields();
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task<int> SaveChangesSystemAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFieldsSystem();
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    private void UpdateAuditFieldsSystem()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is Usuario userCreated)
                    {
                        entry.Entity.UpdateCreationProperties(now, userCreated.Alias);
                    }
                    else
                    {
                        entry.Entity.UpdateCreationProperties(now, "System");
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateModifiedProperties(now, "System");

                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, "System");
                    entry.Entity.UpdateIsDeleted(true);
                    break;
            }
    }

    private void UpdateAuditFields()
    {
        var now = DateTime.UtcNow;
        var nombre = currentUsuarioService.GetAlias();
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is Usuario userCreated)
                    {
                        entry.Entity.UpdateCreationProperties(now, userCreated.Alias);
                    }
                    else
                    {
                        entry.Entity.UpdateCreationProperties(now, nombre);
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateModifiedProperties(now, nombre);

                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, nombre);
                    entry.Entity.UpdateIsDeleted(true);
                    break;
            }
    }
}

public static class Extensions
{
    public static void FilterSoftDeletedRecords(this ModelBuilder modelBuilder)
    {
        Expression<Func<IBaseEntity, bool>> filterExpr = e => !e.IsDeleted;

        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes()
                     .Where(m => m.ClrType.IsAssignableTo(typeof(IBaseEntity))))
        {
            var clrType = mutableEntityType.ClrType;
            var isDeletedProperty = clrType.GetProperty("IsDeleted");

            var uwu = mutableEntityType.GetProperties();

            if (isDeletedProperty != null &&
                mutableEntityType.FindProperty(isDeletedProperty.Name) != null &&  // Verifica que no esté ignorada
                isDeletedProperty.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0)
            {
                var parameter = Expression.Parameter(clrType);
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);

                mutableEntityType.SetQueryFilter(lambdaExpression);
            }
        }
    }
}