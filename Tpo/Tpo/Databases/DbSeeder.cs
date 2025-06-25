using Tpo.Domain.Deporte;
using Tpo.Domain.Deporte.Models;
using Tpo.Domain.Usuario;
using Tpo.Domain.Usuario.Models;

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
                    UsuarioNombre = "string"
                }),
                Usuario.Create(new UsuarioForCreation
                {
                    Nombre = "Ana Gómez",
                    Contrasena = "Ana2024!",
                    Email = "ana.gomez@email.com",
                    Ubicacion = "Rosario",
                    UsuarioNombre = "anag"
                }),
                Usuario.Create(new UsuarioForCreation
                {
                    Nombre = "Carlos Pérez",
                    Contrasena = "Carlos#123",
                    Email = "carlos.perez@email.com",
                    Ubicacion = "Córdoba",
                    UsuarioNombre = "carlosp"
                }),
                Usuario.Create(new UsuarioForCreation
                {
                    Nombre = "Lucía Fernández",
                    Contrasena = "Lucia*456",
                    Email = "lucia.fernandez@email.com",
                    Ubicacion = "Mendoza",
                    UsuarioNombre = "luciaf"
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
        }
    }
}