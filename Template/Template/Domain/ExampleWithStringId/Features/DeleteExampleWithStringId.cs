using MediatR;
using SharedKernel.Databases;
using Template.Domain.ExampleWithStringId.Services;

namespace Template.Domain.ExampleWithStringId.Features;

public static class DeleteExampleWithStringId
{
    public sealed record Command(string Code) : IRequest;

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IExampleWithStringIdRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IExampleWithStringIdRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var entity =
                await _repository.GetById(request.Code, cancellationToken);

            _repository.Remove(entity);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}