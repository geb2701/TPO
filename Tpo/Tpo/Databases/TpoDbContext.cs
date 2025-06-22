using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Tpo.Domain.User;
using Tpo.Services;

namespace Tpo.Databases;

public sealed class TpoDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;
    private readonly DbContextOptions<TpoDbContext> _options;

    public TpoDbContext(DbContextOptions<TpoDbContext> options,
        ICurrentUserService currentUserService, IMediator mediator) : base(options)
    {
        _options = options;
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    public DbSet<User> User { get; set; }
    public DbSet<User> Deporte { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.FilterSoftDeletedRecords();
        /* any query filters added after this will override soft delete
                https://docs.microsoft.com/en-us/ef/core/querying/filters
                https://github.com/dotnet/efcore/issues/10275
        */

        // modelBuilder.ApplyConfiguration(new BaseExampleConfiguration());

        // descomentar para añadir ejemplos
        //modelBuilder.Seed();
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        var result = base.SaveChanges();
        DispatchDomainEvents().GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFields();
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEvents();
        return result;
    }

    private async Task DispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<IBaseEntity>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Count != 0)
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
                await _mediator.Publish(entityDomainEvent);
        }
    }

    private void UpdateAuditFields()
    {
        var now = DateTime.UtcNow;
        
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is User userCreated)
                    {
                        entry.Entity.UpdateCreationProperties(now, userCreated.Name);
                    }
                    else
                    {
                        var userAdd = _currentUserService.GetUser();
                        entry.Entity.UpdateCreationProperties(now, userAdd.Name);
                    }
                    break;

                case EntityState.Modified:
                    var user = _currentUserService.GetUser();
                    if (entry.Entity is IApprovableEntity approvableEntity)
                    {
                        var originalStatus = entry.OriginalValues[nameof(IApprovableEntity.Status)];
                        if (!Equals(originalStatus, approvableEntity.Status))
                        {

                            approvableEntity.UpdateApprovedProperties(now, user.Name);
                        }
                    }
                    else
                    {
                        entry.Entity.UpdateModifiedProperties(now, user.Name);
                    }
                    break;

                case EntityState.Deleted:
                    var userDelete = _currentUserService.GetUser();
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, userDelete.Name);
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