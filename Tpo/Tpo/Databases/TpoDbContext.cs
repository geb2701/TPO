using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Tpo.Domain.Deporte;
using Tpo.Domain.Usuario;
using Tpo.Services;

namespace Tpo.Databases;

public sealed class TpoDbContext(DbContextOptions<TpoDbContext> options,
    ICurrentUsuarioService currentUsuarioService) : DbContext(options)
{
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Deporte> Deporte { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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

    private void UpdateAuditFields()
    {
        var now = DateTime.UtcNow;
        
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is Usuario userCreated)
                    {
                        entry.Entity.UpdateCreationProperties(now, userCreated.UsuarioNombre);
                    }
                    else
                    {
                        var userAdd = currentUsuarioService.GetUsuario();
                        entry.Entity.UpdateCreationProperties(now, userAdd.UsuarioNombre);
                    }
                    break;

                case EntityState.Modified:
                    var user = currentUsuarioService.GetUsuario();
                    if (entry.Entity is IApprovableEntity approvableEntity)
                    {
                        var originalStatus = entry.OriginalValues[nameof(IApprovableEntity.Status)];
                        if (!Equals(originalStatus, approvableEntity.Status))
                        {

                            approvableEntity.UpdateApprovedProperties(now, user.UsuarioNombre);
                        }
                    }
                    else
                    {
                        entry.Entity.UpdateModifiedProperties(now, user.UsuarioNombre);
                    }
                    break;

                case EntityState.Deleted:
                    var userDelete = currentUsuarioService.GetUsuario();
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, userDelete.UsuarioNombre);
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