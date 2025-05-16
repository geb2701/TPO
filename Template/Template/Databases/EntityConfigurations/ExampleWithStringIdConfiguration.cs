using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.ExampleWithStringId;

namespace Template.Databases.EntityConfigurations;

public sealed class ExampleWithStringIdConfiguration : BaseEntityTypeConfiguration<ExampleWithStringId, string>
{
    /// <summary>
    ///     The database configuration for BaseExample.
    /// </summary>
    public override void Configure(EntityTypeBuilder<ExampleWithStringId> builder)
    {
        base.Configure(builder);

        builder.ToTable("ExampleWithStringId");

        builder.HasKey(x => x.Code);

        /*builder.HasMany(x => x.ChildEntities)
            .WithOne(x => x.BaseExample);*/
    }
}