using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tpo.Domain.UsuarioDeporte;

namespace Tpo.Databases.EntityConfigurations
{
    public sealed class UsuarioDeporteConfiguration : BaseEntityTypeConfiguration<UsuarioDeporte, int>
    {
        public override void Configure(EntityTypeBuilder<UsuarioDeporte> builder)
        {
            base.Configure(builder);

            builder.ToTable("UsuarioDeporte");

            builder.HasKey(x => x.Id);

        }
    }
}
