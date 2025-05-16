using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entity;

namespace Template.Databases.EntityConfigurations
{
    /// <summary>
    /// Configuración base para entidades.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class BaseEntityTypeConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (typeof(TKey) == typeof(Guid))
            {
                builder.HasKey(x => x.Id).IsClustered(false);
            }
            else
            {
                builder.HasKey(x => x.Id);
            }

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()"); // MSSQL
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        }
    }

    /// <summary>
    /// Configuración para entidades aprobables.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class ApprovableEntityTypeConfiguration<TEntity, TKey> : BaseEntityTypeConfiguration<TEntity, TKey>, IEntityTypeConfiguration<TEntity> where TEntity : ApprovableEntity<TKey>
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        }
    }

    /// <summary>
    /// Configuración para entidades versionables.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class VersionableEntityTypeConfiguration<TEntity, TKey> : BaseEntityTypeConfiguration<TEntity, TKey>, IEntityTypeConfiguration<TEntity> where TEntity : VersionableEntity<TKey>
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.Property(x => x.Version).HasDefaultValue(1);
        }
    }

    /// <summary>
    /// Configuración para entidades aprobables y versionables.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad.</typeparam>
    /// <typeparam name="TKey">Tipo de la clave primaria de la entidad.</typeparam>
    public abstract class ApprovableVersionableEntityTypeConfiguration<TEntity, TKey> : ApprovableEntityTypeConfiguration<TEntity, TKey> where TEntity : ApprovableVersionableEntity<TKey>
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.Property(x => x.Version).HasDefaultValue(1);
        }
    }
}



