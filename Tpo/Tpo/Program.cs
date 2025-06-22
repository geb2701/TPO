using Destructurama;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Globalization;
using System.Reflection;
using System.Text;
using Tpo.Extensions.Application;
using Tpo.Extensions.Services;
using Tpo.Middleware;
using Tpo.Services.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Set default culture to a specific one (e.g., "en-US" or "es-AR")
var defaultCulture = new CultureInfo("es-AR");

// Customize the culture as needed
defaultCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

// Customize your date format
defaultCulture.DateTimeFormat.LongDatePattern = "dd MMMM, yyyy";

CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName)
    .Destructure.UsingAttributes()
    .CreateLogger();

// CORS
/*
var allowedOrigins = builder.Configuration["AllowedOrigins"];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(allowedOrigins.Split(','));
            policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((host) => true);
        });
});
*/

builder.Host.UseSerilog();

/* CONFIGURACIÓN DE SERVICIOS */
builder.ConfigureServices();

/*
builder.Services.AddControllers(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>()).AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonEnumObjectConverterFactory()));
*/

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
        )
           .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   IssuerSigningKey =
                       new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtUtils.JwtSecretKey))
               };
           }
        );

var app = builder.Build();

using var scope = app.Services.CreateScope();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseProblemDetails();

app.UseStaticFiles();

// For elevated security, it is recommended to remove this middleware and set your server to only listen on https.
// A slightly less secure option would be to redirect http to 400, 505, etc.
app.UseHttpsRedirection();

// CORS
app.UseCors("CorsPolicy");

app.UseHealthCheck(builder.Environment);
app.UseSerilogRequestLogging();
app.UseRouting();

app.MapControllers();
app.UseMiddleware<AuthMiddleware>();
app.UseSwaggerExtension(builder.Configuration, builder.Environment);

// Note: Switch between Prometheus/OTLP/Console by setting UseMetricsExporter in appsettings.json.
var metricsExporter = builder.Configuration.GetValue("UseMetricsExporter", defaultValue: "console")!.ToLowerInvariant();

try
{
    Log.Warning("[!!] Starting application");
    await app.RunAsync();
}
catch (Exception e)
{
    Log.Error(e, "[!!] The application failed to start correctly");
    throw;
}
finally
{
    Log.Warning("[!!] Shutting down application");
    Log.CloseAndFlush();
}

// Make the implicit Program class public so the functional test project can access it
public partial class Program
{
}