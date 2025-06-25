using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tpo.Domain.Deporte;

namespace Tpo.Databases.EntityConfigurations
{
    public sealed class DeporteConfiguration : BaseEntityTypeConfiguration<Deporte, int>
    {
        public override void Configure(EntityTypeBuilder<Deporte> builder)
        {
            base.Configure(builder);

            builder.ToTable("Deporte");

            builder.HasKey(x => x.Id);

            builder
                .HasMany(u => u.UsuariosDeportes)
                .WithOne(ud => ud.Deporte)
                .HasForeignKey(ud => ud.DeporteId);
        }
    }
}
