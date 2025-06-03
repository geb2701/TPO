using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.ExampleWithIntId;

namespace Template.Databases.EntityConfigurations;

public sealed class ExampleWithIntIdConfiguration : BaseEntityTypeConfiguration<User, int>
{
    /// <summary>
    ///     The database configuration for BaseExample.
    /// </summary>
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("ExampleWithIntId");

        builder.HasKey(x => x.Id);

        /*builder.HasMany(x => x.ChildEntities)
            .WithOne(x => x.BaseExample);*/
    }
}