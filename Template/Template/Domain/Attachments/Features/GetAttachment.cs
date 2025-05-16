using MediatR;
using Template.Domain.Attachments.Dtos;
using Template.Domain.Attachments.Mappings;
using Template.Domain.Attachments.Services;

namespace Template.Domain.Attachments.Features;

public static class GetAttachment
{
    public sealed record Query(Guid AttachmentId) : IRequest<AttachmentContentDto>;

    public sealed class Handler
        : IRequestHandler<Query, AttachmentContentDto>
    {
        private readonly IAttachmentRepository _attachmentRepository;

        public Handler(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public async Task<AttachmentContentDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _attachmentRepository.GetById(request.AttachmentId, cancellationToken);
            return result.ToAttachmentContentDto();
        }
    }
}