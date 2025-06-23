using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entity;

namespace Tpo.Databases.EntityConfigurations
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

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        }
    }
}



