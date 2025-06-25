using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tpo.Domain.Partido;

namespace Tpo.Databases.EntityConfigurations
{
    public sealed class PartidoConfiguration : BaseEntityTypeConfiguration<Partido, int>
    {
        public override void Configure(EntityTypeBuilder<Partido> builder)
        {
            base.Configure(builder);

            builder.ToTable("Partido");

            builder.HasKey(x => x.Id);

            var estadoConverter = new ValueConverter<IPartidoState, string>(
                v => v.Nombre,
                v => PartidoStateParser.Parse(v)
            );

            var estrategiaConverter = new ValueConverter<IEstrategiaEmparejamiento, string>(
               v => EstrategiaEmparejamientoParser.GetNombre(v),
               v => EstrategiaEmparejamientoParser.Parse(v)
            );

            builder
                .Property(p => p.Estado)
                .HasConversion(estadoConverter)
                .HasColumnName("Estado")
                .IsRequired();

            builder
                .Property(p => p.EstrategiaEmparejamiento)
                .HasConversion(estrategiaConverter)
                .HasColumnName("EstrategiaEmparejamiento")
                .IsRequired();

            builder
                .HasMany(u => u.Jugadores)
                .WithOne(ud => ud.Partido)
                .HasForeignKey(ud => ud.PartidoId);
        }

        private static class EstrategiaEmparejamientoParser
        {
            public static IEstrategiaEmparejamiento Parse(string nombre)
            {
                return nombre switch
                {
                    "PorNivel" => new EmparejamientoPorNivel(),
                    "PorCercania" => new EmparejamientoPorCercania(),
                    "PorHistorial" => new EmparejamientoPorHistorial(),
                    _ => throw new ArgumentException($"Estrategia desconocida: {nombre}")
                };
            }

            public static string GetNombre(IEstrategiaEmparejamiento estrategia)
            {
                return estrategia switch
                {
                    EmparejamientoPorNivel => "PorNivel",
                    EmparejamientoPorCercania => "PorCercania",
                    EmparejamientoPorHistorial => "PorHistorial",
                    _ => throw new ArgumentException($"Estrategia desconocida: {estrategia.GetType().Name}")
                };
            }
        }

        private static class PartidoStateParser
        {
            public static IPartidoState Parse(string nombre)
            {
                return nombre switch
                {
                    "Necesitamos jugadores" => new NecesitamosJugadoresState(),
                    "Partido armado" => new PartidoArmadoState(),
                    "Confirmado" => new ConfirmadoState(),
                    "En juego" => new EnJuegoState(),
                    "Finalizado" => new FinalizadoState(),
                    "Cancelado" => new CanceladoState(),
                    _ => throw new ArgumentException($"Estado desconocido: {nombre}")
                };
            }
        }
    }
}
