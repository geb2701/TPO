using MediatR;
using SharedKernel.Databases;
using Template.Domain.ExampleWithIntId.Services;

namespace Template.Domain.ExampleWithIntId.Features;

public static class DeleteExampleWithIntId
{
    public sealed record Command(int Id) : IRequest;

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IExampleWithIntIdRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IExampleWithIntIdRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var entity =
                await _repository.GetById(request.Id, cancellationToken);

            _repository.Remove(entity);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}