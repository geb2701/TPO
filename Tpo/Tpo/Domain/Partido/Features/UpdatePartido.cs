using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Mappings;
using Tpo.Domain.Partido.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.Partido.Features;

public class UpdatePartido
{
    public sealed record Command(int Id, PartidoForUpdateDto Dto) : IRequest;

    public class UpdatePartidoValidator : AbstractValidator<Command>
    {
        public UpdatePartidoValidator()
        {
            RuleFor(x => x.Dto.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IPartidoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UpdatePartidoValidator _validator;

        public Handler(IPartidoRepository repository, IUnitOfWork unitOfWork,
            UpdatePartidoValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrowValidationException(request);

            var entity = await _repository.GetById(request.Id, true, cancellationToken);
            var model = request.Dto.ToPartidoForUpdate();

            //entity.Update(model);

            _repository.Update(entity);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}
