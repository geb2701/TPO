using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tpo.Domain.Usuario;

namespace Tpo.Databases.EntityConfigurations
{
    public sealed class UsuarioConfiguration : BaseEntityTypeConfiguration<Usuario, int>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            base.Configure(builder);

            builder.ToTable("Usuario");

            builder.HasKey(x => x.Id);

            builder
                .HasMany(u => u.Habilidades)
                .WithOne(ud => ud.Usuario)
                .HasForeignKey(ud => ud.UsuarioId);

            builder
                .HasMany(u => u.Participante)
                .WithOne(ud => ud.Usuario)
                .HasForeignKey(ud => ud.UsuarioId);
        }
    }
}
