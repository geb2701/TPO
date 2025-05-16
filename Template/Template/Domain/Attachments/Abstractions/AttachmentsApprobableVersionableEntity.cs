using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using Template.Domain.Attachments.Models;

namespace Template.Domain.Attachments.Abstractions
{
    public abstract class AttachmentsApprobableVersionableEntity : ApprovableVersionableEntity<Guid>, IAttachmentsEntity
    {
        private List<Attachment> _attachments = new();

        [NotMapped]
        public ICollection<Attachment> ListaAttachments
        {
            get => _attachments;
            set => _attachments = (List<Attachment>)value;
        }

        public void DeleteAttachments()
        {
            foreach (var attachment in ListaAttachments)
                if (attachment.FromVersion == Version)
                    attachment.Delete();
                else if (attachment.ToVersion == Version)
                    attachment.Update(new AttachmentForUpdate
                    {
                        OwnerId = OriginKey,
                        OwnerType = GetType().Name,
                        FromVersion = attachment.FromVersion,
                        ToVersion = null
                    });
        }

        public void SetAttachments(List<Attachment> lista)
        {
            {
                var eliminadosVersionable =
                    ListaAttachments.Where(x => !lista.Any(y => y.Id == x.Id) && x.ToVersion == null);
                foreach (var attachment in eliminadosVersionable)
                    if (Version != 1)
                        if (attachment.FromVersion != Version)
                            attachment.UpdateToVersion(Version - 1);
                        else
                            attachment.Delete();
                    else
                        attachment.Delete();

                var agregadosVersionable = lista.Where(x => !ListaAttachments.Any(y => y.Id == x.Id));
                foreach (var attachment in agregadosVersionable)
                {
                    attachment.Update(new AttachmentForUpdate
                    {
                        OwnerId = OriginKey,
                        OwnerType = GetType().Name,
                        FromVersion = Version,
                        ToVersion = null
                    });
                    ListaAttachments.Add(attachment);
                }
            }
        }
    }
}
