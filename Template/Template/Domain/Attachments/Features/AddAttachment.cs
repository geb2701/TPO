using MediatR;
using SharedKernel.Databases;
using Template.Domain.Attachments.Dtos;
using Template.Domain.Attachments.Mappings;
using Template.Domain.Attachments.Services;

namespace Template.Domain.Attachments.Features;

public static class AddAttachment
{
    public sealed record Command(AttachmentForCreationDto AttachmentToAdd) : IRequest<AttachmentDto>;

    public sealed class Handler
        : IRequestHandler<Command, AttachmentDto>
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IAttachmentRepository attachmentRepository, IUnitOfWork unitOfWork)
        {
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AttachmentDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var attachmentToAdd = request.AttachmentToAdd.ToAttachmentForCreation();
            var attachment = await Attachment.Create(attachmentToAdd);

            await _attachmentRepository.Add(attachment, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);

            return attachment.ToAttachmentDto();
        }
    }
}