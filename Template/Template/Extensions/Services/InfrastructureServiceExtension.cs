using Microsoft.EntityFrameworkCore;
using Template.Configurations;
using Template.Databases;

namespace Template.Extensions.Services;

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
        var connectionString = configuration.GetConnectionStringOptions().Template;
        var useDbInMemory = configuration.GetValue<bool>("DbInMemory", false);

        if (useDbInMemory && env.IsDevelopment())
        {
            // In-memory database for testing in development
            services.AddDbContext<TemplateDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));
        }
        else
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                // this makes local migrations easier to manage. feel free to refactor if desired.
                connectionString = env.IsDevelopment()
                    ? "Data Source=localhost,63878;Integrated Security=False;Database=dev_template;User ID=SA;Password=#localDockerPassword#"
                    : throw new Exception("The database connection string is not set.");

            services.AddDbContext<TemplateDbContext>(options =>
                options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly(typeof(TemplateDbContext).Assembly.FullName)));
            //.UseSnakeCaseNamingConvention());
        }

        // Execute migrations on startup
        // https://learn.microsoft.com/es-es/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
        //services.AddHostedService<MigrationHostedService<TemplateDbContext>>();
    }
}