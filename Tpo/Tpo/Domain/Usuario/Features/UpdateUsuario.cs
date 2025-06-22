using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Usuario.Mappings;
using Tpo.Domain.Usuario.Dtos;
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
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.Dto.Ubicacion)
                .NotEmpty().WithMessage("La ubicación es obligatoria.")
                .MaximumLength(100).WithMessage("La ubicación no debe superar los 100 caracteres.");
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UpdateUsuarioValidator _validator;

        public Handler(IUsuarioRepository repository, IUnitOfWork unitOfWork,
            UpdateUsuarioValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrowValidationException(request);

            var entity =
                await _repository.GetById(request.Id, cancellationToken);
            var model = request.Dto.ToUsuarioForUpdate();

            entity.Update(model);

            _repository.Update(entity);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}