using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Tpo.Domain.Usuario;
using Tpo.Domain.Usuario.Services;

namespace Tpo.Services.Jwt;


/// <summary>
/// Interfaz para utilidades relacionadas con JWT, como generación, validación y carga de tokens.
/// </summary>
public interface IJwtUtils: IScopedService
{
    /// <summary>
    /// Genera un token JWT a partir de un usuario.
    /// </summary>
    /// <param name="user">Usuario para el cual se genera el token.</param>
    /// <returns>Token JWT como cadena.</returns>
    string GenerateJwtToken(Usuario user);

    /// <summary>
    /// Valida un token JWT y retorna el nombre de usuario si es válido.
    /// </summary>
    /// <param name="token">Token JWT a validar.</param>
    /// <returns>Nombre de usuario si es válido, o null si no lo es.</returns>
    Task<string?> ValidateJwtTokenAsync(string? token);

    /// <summary>
    /// Valida un token JWT y retorna true si es válido.
    /// </summary>
    /// <param name="token">Token JWT a validar.</param>
    /// <returns>True si el token es válido, false en caso contrario.</returns>
    Task<bool> ValidationJwtTokenAsync(string? token);

    /// <summary>
    /// Decodifica y valida un token JWT, devolviendo el resultado de la validación.
    /// </summary>
    /// <param name="token">Token JWT a decodificar.</param>
    /// <returns>Resultado de la validación del token.</returns>
    Task<TokenValidationResult?> DecodeToken(string token);

    /// <summary>
    /// Carga los datos del token JWT en la instancia de usuario proporcionada.
    /// </summary>
    /// <param name="token">Token JWT a cargar.</param>
    /// <param name="user">Instancia de usuario donde se cargarán los datos.</param>
    Task LoadToken(string token, Usuario user);
}

/// <summary>
/// Implementación de <see cref="IJwtUtils"/> para la gestión de tokens JWT.
/// Permite generar, validar y cargar información de usuarios desde tokens JWT.
/// </summary>
internal class JwtUtils(IUsuarioRepository userRepository) : IJwtUtils
{
    public static readonly string JwtSecretKey = RandomStringGenerator.GenerateRandomString(32);
    /// <inheritdoc/>
    public string GenerateJwtToken(Usuario user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtSecretKey);
        var claims = new List<Claim>();

        PropertyInfo[] properties = typeof(Usuario).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            if (property.CanRead && property.CanWrite)
            {
                var value = property.GetValue(user);
                claims.Add(new Claim(property.Name, value != null ? JsonSerializer.Serialize(value)! : ""));
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var encode = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(encode);

        return token;
    }

    /// <inheritdoc/>
    public async Task<string?> ValidateJwtTokenAsync(string? token)
    {
        if (token == null)
            return null;

        try
        {
            var validatedToken = await DecodeToken(token);
            var username = validatedToken?.Claims.First(x => x.Key == nameof(Usuario.UsuarioNombre)).Value.ToString();
            var password = validatedToken?.Claims.First(x => x.Key == nameof(Usuario.Contrasena)).Value.ToString();

            var users = userRepository.Query(x => x.UsuarioNombre == username && x.Contrasena == password);

            if (users.Count() != 1)
            {
                return null;
            }

            return users.FirstOrDefault().UsuarioNombre;
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ValidationJwtTokenAsync(string? token)
    {
        if (token == null)
            return false;

        try
        {
            var validatedToken = await DecodeToken(token);
            var username = validatedToken?.Claims.First(x => x.Key == nameof(Usuario.UsuarioNombre)).Value.ToString();
            var password = validatedToken?.Claims.First(x => x.Key == nameof(Usuario.Contrasena)).Value.ToString();

            var users = userRepository.Query(x => x.UsuarioNombre == username && x.Contrasena == password);

            if (users.Count() != 1)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task LoadToken(string token, Usuario user)
    {
        var validatedToken = await DecodeToken(token);

        // Asigna los valores de los claims a las propiedades del usuario
        PropertyInfo[] properties = typeof(Usuario).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {

            if (property.CanRead && property.CanWrite)
            {
                var claim = validatedToken?.Claims.FirstOrDefault(x => x.Key == property.Name);
                if (claim != null)
                {
                    var value = claim.Value.Value.ToString();

                    if (value != null && !string.IsNullOrWhiteSpace(value))
                    {
                        property.SetValue(user, JsonSerializer.Deserialize(value, property.PropertyType));
                    }
                }
            }
        }
    }

    /// <inheritdoc/>
    public async Task<TokenValidationResult?> DecodeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtSecretKey);
        var validatedToken = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RequireAudience = true,
            ClockSkew = TimeSpan.Zero
        });

        return validatedToken;
    }

    internal static class RandomStringGenerator
    {
        private static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        private static readonly Random random = new();

        public static string GenerateRandomString(int length)
        {
            Console.WriteLine("Warning: Secret key generated. Only use for development.");
            return new string([.. Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)])]);
        }
    }
}
