using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.UsuarioDeporte.Dtos;
using Tpo.Domain.UsuarioDeporte.Mappings;
using Tpo.Domain.UsuarioDeporte.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.UsuarioDeporte.Features;

public class UpdateUsuarioDeporte
{
    public sealed record Command(int Id, UsuarioDeporteForUpdateDto Dto) : IRequest<UsuarioDeporteDto>;

    public class AddUsuarioDeporteValidator : AbstractValidator<Command>
    {
        public AddUsuarioDeporteValidator()
        {
            RuleFor(x => x.Dto.Nivel)
                .IsInEnum().WithMessage("El nivel de habilidad no es válido.");
        }
    }

    public sealed class Handler(IUsuarioDeporteRepository repository, IUnitOfWork unitOfWork, AddUsuarioDeporteValidator validator) : IRequestHandler<Command, UsuarioDeporteDto>
    {
        public async Task<UsuarioDeporteDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var entity = await repository.GetById(request.Id, cancellationToken, includes: [x => x.Deporte, x => x.Usuario]);

            var model = request.Dto.ToUsuarioDeporteForUpdate();

            entity.Update(model);

            var registers = repository.Query(includes: [x => x.Deporte, x => x.Usuario]);

            if (registers.Any(x => x.Usuario.Id == entity.Usuario.Id && x.Favorito && entity.Favorito && x.Id != entity.Id))
            {
                throw new ArgumentException($"El usuario ya tiene otro deporte como favorito.");
            }

            await unitOfWork.CommitChanges(cancellationToken);
            return entity.ToUsuarioDeporteDto();
        }
    }
}
