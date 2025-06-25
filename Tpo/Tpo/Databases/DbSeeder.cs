using Tpo.Domain;
using Tpo.Domain.Deporte;
using Tpo.Domain.Deporte.Models;
using Tpo.Domain.Usuario;
using Tpo.Domain.Usuario.Models;
using Tpo.Domain.UsuarioDeporte;
using Tpo.Domain.UsuarioDeporte.Models;

namespace Tpo.Databases
{
    public static class DbSeeder
    {
        public static async Task DbSeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TpoDbContext>();

            var usuarios = new List<Usuario>
            {
                Usuario.Create(new UsuarioForCreation
                {
                    Nombre = "string",
                    Contrasena = "string",
                    Email = "string@a.a",
                    Ubicacion = "CABA",
                    Alias = "string",
                    TipoNotificacion = TipoNotificacion.PushCelular
                }),
                Usuario.Create(new UsuarioForCreation
                {
                    Nombre = "Ana Gómez",
                    Contrasena = "Ana2024!",
                    Email = "ana.gomez@email.com",
                    Ubicacion = "Rosario",
                    Alias = "anag",
                    TipoNotificacion = TipoNotificacion.Ninguna
                }),
                Usuario.Create(new UsuarioForCreation
                {
                    Nombre = "Carlos Pérez",
                    Contrasena = "Carlos#123",
                    Email = "carlos.perez@email.com",
                    Ubicacion = "Córdoba",
                    Alias = "carlosp",
                    TipoNotificacion = TipoNotificacion.InApp
                }),
                Usuario.Create(new UsuarioForCreation
                {
                    Nombre = "Lucía Fernández",
                    Contrasena = "Lucia*456",
                    Email = "lucia.fernandez@email.com",
                    Ubicacion = "Mendoza",
                    Alias = "luciaf",
                    TipoNotificacion = TipoNotificacion.EmailYPush
                })
            };

            context.Usuario.AddRange(usuarios);
            await context.SaveChangesSystemAsync();

            var deportes = new List<Deporte>
            {
                Deporte.Create(new DeporteForCreation { Nombre = "Fútbol" }),
                Deporte.Create(new DeporteForCreation { Nombre = "Básquet" }),
                Deporte.Create(new DeporteForCreation { Nombre = "Tenis" })
            };

            context.Deporte.AddRange(deportes);
            await context.SaveChangesSystemAsync();

            usuarios = [.. context.Usuario];
            deportes = [.. context.Deporte];

            var usuarioDeportes = new List<UsuarioDeporte>
            {
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[0],
                    Deporte = deportes[0],
                    Nivel = NivelHabilidad.Principiante,
                    Favorito = true
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[0],
                    Deporte = deportes[1],
                    Nivel = NivelHabilidad.Intermedio,
                    Favorito = false
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[0],
                    Deporte = deportes[2],
                    Nivel = NivelHabilidad.Avanzado,
                    Favorito = false
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[1],
                    Deporte = deportes[0],
                    Nivel = NivelHabilidad.Intermedio,
                    Favorito = true
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[1],
                    Deporte = deportes[1],
                    Nivel = NivelHabilidad.Principiante,
                    Favorito = false
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[1],
                    Deporte = deportes[2],
                    Nivel = NivelHabilidad.Avanzado,
                    Favorito = true
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[2],
                    Deporte = deportes[0],
                    Nivel = NivelHabilidad.Avanzado,
                    Favorito = false
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[2],
                    Deporte = deportes[1],
                    Nivel = NivelHabilidad.Intermedio,
                    Favorito = true
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[2],
                    Deporte = deportes[2],
                    Nivel = NivelHabilidad.Principiante,
                    Favorito = false
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[3],
                    Deporte = deportes[0],
                    Nivel = NivelHabilidad.Principiante,
                    Favorito = true
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[3],
                    Deporte = deportes[1],
                    Nivel = NivelHabilidad.Avanzado,
                    Favorito = false
                }),
                UsuarioDeporte.Create(new UsuarioDeporteForCreation
                {
                    Usuario = usuarios[3],
                    Deporte = deportes[2],
                    Nivel = NivelHabilidad.Intermedio,
                    Favorito = true
                })
            };

            context.UsuarioDeporte.AddRange(usuarioDeportes);

            await context.SaveChangesSystemAsync();
        }
    }
}