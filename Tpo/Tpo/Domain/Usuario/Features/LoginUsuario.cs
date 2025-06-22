using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Usuario.Dtos;
using Tpo.Domain.Usuario.Mappings;
using Tpo.Domain.Usuario.Services;
using Tpo.Exceptions;
using Tpo.Extensions.Application;
using Tpo.Services.Jwt;

namespace Tpo.Domain.Usuario.Features;

public class LoginUsuario
{
    public sealed record Command(UsuarioLoginDto Dto) : IRequest<string>;

    public class LoginValidator : AbstractValidator<Command>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Dto.Name).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler(IUsuarioRepository repository, IJwtUtils jwtUtils,
            LoginValidator validator) : IRequestHandler<Command, string>
    {
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var user = repository.Query(x => x.UsuarioNombre == request.Dto.Name && x.Contrasena == request.Dto.Password).FirstOrDefault();

            return Task.FromResult(user is null
                ? throw new NotFoundException("Usuario no encontrado o contraseña incorrecta.")
                : (jwtUtils.GenerateJwtToken(user)
                ?? throw new NotFoundException("Error al generar el token JWT.")));
        }
    }
}