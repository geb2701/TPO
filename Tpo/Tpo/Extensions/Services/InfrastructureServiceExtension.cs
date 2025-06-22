using Microsoft.EntityFrameworkCore;
using Tpo.Configurations;
using Tpo.Databases;

namespace Tpo.Extensions.Services;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IWebHostEnvironment env,
        IConfiguration configuration)
    {
        /* SETUP DATABASE */
        services.SetupDatabase(env, configuration);

        /* SETUP HANGFIRE */
        //services.SetupHangfire(env);
    }
}

public static class DatabaseConfig
{
    public static void SetupDatabase(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionStringOptions().Tpo;
        var useDbInMemory = configuration.GetValue<bool>("DbInMemory", true);

        if (useDbInMemory)
        {
            // In-memory database for testing in development
            services.AddDbContext<TpoDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));
        }
        else
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                // this makes local migrations easier to manage. feel free to refactor if desired.
                connectionString = env.IsDevelopment()
                    ? "Data Source=localhost,63878;Integrated Security=False;Database=dev_template;Usuario ID=SA;Password=#localDockerPassword#"
                    : throw new Exception("The database connection string is not set.");

            services.AddDbContext<TpoDbContext>(options =>
                options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly(typeof(TpoDbContext).Assembly.FullName)));
            //.UseSnakeCaseNamingConvention());
        }
    }
}