using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Template.Databases.EntityConfigurations;
using Template.Domain.Attachments;
using Template.Domain.ExampleWithIntId;
using Template.Domain.ExampleWithStringId;
using Template.Services;

namespace Template.Databases;

public sealed class TemplateDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;
    private readonly DbContextOptions<TemplateDbContext> _options;

    public TemplateDbContext(DbContextOptions<TemplateDbContext> options,
        ICurrentUserService currentUserService, IMediator mediator) : base(options)
    {
        _options = options;
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    public DbSet<User> ExamplesWithIntId { get; set; }
    public DbSet<ExampleWithStringId> ExamplesWithStringId { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.FilterSoftDeletedRecords();
        /* any query filters added after this will override soft delete
                https://docs.microsoft.com/en-us/ef/core/querying/filters
                https://github.com/dotnet/efcore/issues/10275
        */

        // modelBuilder.ApplyConfiguration(new BaseExampleConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExampleWithStringIdConfiguration).Assembly);

        // descomentar para añadir ejemplos
        //modelBuilder.Seed();
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        var result = base.SaveChanges();
        _dispatchDomainEvents().GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFields();
        var result = await base.SaveChangesAsync(cancellationToken);
        await _dispatchDomainEvents();
        return result;
    }

    private async Task _dispatchDomainEvents()
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
                    entry.Entity.UpdateCreationProperties(now, _currentUserService?.UserId);
                    break;

                case EntityState.Modified:

                    if (entry.Entity is IApprovableEntity approvableEntity)
                    {
                        var originalStatus = entry.OriginalValues[nameof(IApprovableEntity.Status)];
                        if (!Equals(originalStatus, approvableEntity.Status))
                        {
                            approvableEntity.UpdateApprovedProperties(now, _currentUserService?.UserId);
                        }
                    }
                    else
                    {
                        entry.Entity.UpdateModifiedProperties(now, _currentUserService?.UserId);
                    }
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, _currentUserService?.UserId);
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
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var examples = new List<ExampleWithStringId>();

        for (int i = 1; i <= 100; i++)
        {
            examples.Add(ExampleWithStringId.Create(new Domain.ExampleWithStringId.Models.ExampleWithStringIdForCreation()
            {
                Code = $"Codigo {i}",
                Name = $"Nombre {i}"
            }));
        }

        modelBuilder.Entity<ExampleWithStringId>().HasData(examples);
    }
}