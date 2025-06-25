using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Usuario.Dtos;
using Tpo.Domain.Usuario.Mappings;
using Tpo.Domain.Usuario.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.Usuario.Features;

public class AddUsuario
{
    public sealed record Command(UsuarioForCreationDto Dto) : IRequest<UsuarioDto>;

    public class AddUsuarioValidator : AbstractValidator<Command>
    {
        public AddUsuarioValidator()
        {
            RuleFor(x => x.Dto.Alias)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .Length(3, 30).WithMessage("El nombre de usuario debe tener entre 3 y 30 caracteres.")
                .Matches("^[^\\s]+$").WithMessage("El nombre de usuario no debe contener espacios.");


            RuleFor(x => x.Dto.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.");

            RuleFor(x => x.Dto.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.");

            RuleFor(x => x.Dto.Ubicacion)
                .NotEmpty().WithMessage("La ubicación es obligatoria.")
                .MaximumLength(100).WithMessage("La ubicación no debe superar los 100 caracteres.");

            RuleFor(x => x.Dto.TipoNotificacion)
                .IsInEnum()
                .WithMessage("El tipo de notificación seleccionado no es válido.");
        }
    }

    public sealed class Handler(IUsuarioRepository repository, IUnitOfWork unitOfWork,
            AddUsuarioValidator validator) : IRequestHandler<Command, UsuarioDto>
    {
        public async Task<UsuarioDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var model = request.Dto.ToUsuarioForCreation();
            var entity = Usuario.Create(model);

            if (repository.Query().Any(x => x.Alias == entity.Alias || x.Email == entity.Email))
                throw new ApplicationException("El usuario ya existe.");

            await repository.Add(entity, cancellationToken);
            await unitOfWork.CommitChanges(cancellationToken);

            return entity.ToUsuarioDto();
        }
    }
}