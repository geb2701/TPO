using Riok.Mapperly.Abstractions;
using Template.Domain.Attachments.Dtos;
using Template.Domain.Attachments.Models;

namespace Template.Domain.Attachments.Mappings;

[Mapper]
public static partial class AttachmentMapper
{
    public static partial AttachmentForCreation ToAttachmentForCreation(
        this AttachmentForCreationDto attachmentForCreationDto);

    /*public static partial AttachmentForUpdate ToAttachmentForUpdate(
        this AttachmentForUpdateDto attachmentForUpdateDto);*/
    public static partial AttachmentDto ToAttachmentDto(
        this Attachment attachment);

    public static partial AttachmentContentDto ToAttachmentContentDto(
        this Attachment attachment);

    public static partial ICollection<AttachmentDto> ToAttachmentDtoCollection(
        this ICollection<Attachment> attachment);
}