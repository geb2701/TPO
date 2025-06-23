using FluentValidation;
using MediatR;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Mappings;
using Tpo.Domain.Partido.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.Partido.Features;

public class AddPartido
{
    public sealed record Command(PartidoForCreationDto Dto) : IRequest<PartidoDto>;

    public class AddDeporteValidator : AbstractValidator<Command>
    {
        public AddDeporteValidator()
        {
            RuleFor(x => x.Dto.Nombre).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler(IPartidoRepository repository, IUnitOfWork unitOfWork,
            AddPartidoValidator validator) : IRequestHandler<Command, PartidoDto>
    {
        public async Task<PartidoDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var model = request.Dto.ToPartidoForCreation();
            var entity = Partido.Create(model);

            if (repository.Query().Any(x => x.Nombre == entity.Nombre))
                throw new ApplicationException("El Partido ya existe.");

            await repository.Add(entity, cancellationToken);
            await unitOfWork.CommitChanges(cancellationToken);

            return entity.ToPartidoDto();
        }
    }
}