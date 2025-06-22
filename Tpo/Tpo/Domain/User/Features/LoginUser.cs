using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.User.Dtos;
using Tpo.Domain.User.Mappings;
using Tpo.Domain.User.Services;
using Tpo.Exceptions;
using Tpo.Extensions.Application;
using Tpo.Services.Jwt;

namespace Tpo.Domain.User.Features;

public class LoginUser
{
    public sealed record Command(UserLoginDto Dto) : IRequest<string>;

    public class LoginValidator : AbstractValidator<Command>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Dto.Name).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler(IUserRepository repository, IJwtUtils jwtUtils,
            LoginValidator validator) : IRequestHandler<Command, string>
    {
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var user = repository.Query(x => x.Name == request.Dto.Name && x.Password == request.Dto.Password).FirstOrDefault();

            return Task.FromResult(user is null
                ? throw new NotFoundException("Usuario no encontrado o contraseña incorrecta.")
                : (jwtUtils.GenerateJwtToken(user)
                ?? throw new NotFoundException("Error al generar el token JWT.")));
        }
    }
}