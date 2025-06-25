using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Usuario.Dtos;
using Tpo.Domain.Usuario.Mappings;
using Tpo.Domain.Usuario.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.Usuario.Features;

public class UpdateUsuario
{
    public sealed record Command(int Id, UsuarioForUpdateDto Dto) : IRequest;

    public class UpdateUsuarioValidator : AbstractValidator<Command>
    {
        public UpdateUsuarioValidator()
        {

            RuleFor(x => x.Dto.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.");

            RuleFor(x => x.Dto.Contrasena)
                .NotEmpty().WithMessage("La contrase�a es obligatoria.")
                .MinimumLength(6).WithMessage("La contrase�a debe tener al menos 6 caracteres.");

            RuleFor(x => x.Dto.Ubicacion)
                .NotEmpty().WithMessage("La ubicaci�n es obligatoria.")
                .MaximumLength(100).WithMessage("La ubicaci�n no debe superar los 100 caracteres.");
        }
    }

    public sealed class Handler(IUsuarioRepository repository, IUnitOfWork unitOfWork,
        UpdateUsuarioValidator validator) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var entity =
                await repository.GetById(request.Id, true, cancellationToken);
            var model = request.Dto.ToUsuarioForUpdate();

            entity.Update(model);

            repository.Update(entity);
            await unitOfWork.CommitChanges(cancellationToken);
        }
    }
}