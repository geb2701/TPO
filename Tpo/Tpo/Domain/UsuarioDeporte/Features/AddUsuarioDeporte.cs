using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.UsuarioDeporte.Mappings;
using Tpo.Domain.UsuarioDeporte.Dtos;
using Tpo.Domain.UsuarioDeporte.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.UsuarioDeporte.Features;

public class AddUsuarioDeporte
{
    public sealed record Command(UsuarioDeporteForCreationDto Dto) : IRequest<int>;

    public class AddUsuarioDeporteValidator : AbstractValidator<Command>
    {
        public AddUsuarioDeporteValidator()
        {
            RuleFor(x => x.Dto.UsuarioId).NotEmpty();
            RuleFor(x => x.Dto.DeporteId).NotEmpty();
            RuleFor(x => x.Dto.Nivel).InclusiveBetween(1, 10);
        }
    }

    public sealed class Handler : IRequestHandler<Command, int>
    {
        private readonly IUsuarioDeporteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AddUsuarioDeporteValidator _validator;

        public Handler(IUsuarioDeporteRepository repository, IUnitOfWork unitOfWork,
            AddUsuarioDeporteValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrowValidationException(request);
            var model = request.Dto.ToUsuarioDeporteForCreation();

            var entity = UsuarioDeporte.Create(model.UsuarioId, model.DeporteId, model.Nivel);

            _repository.Add(entity);
            await _unitOfWork.CommitChanges(cancellationToken);
            return entity.Id;
        }
    }
}
