using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using Template.Domain.Attachments.Models;

namespace Template.Domain.Attachments.Abstractions
{
    public abstract class AttachmentsApprovableEntity : ApprovableEntity<Guid>, IAttachmentsEntity
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
            foreach (var attachment in ListaAttachments) attachment.Delete();
        }

        public void SetAttachments(List<Attachment> lista)
        {
            {
                var eliminadosApprovable =
                    ListaAttachments.Where(x => !lista.Any(y => y.Id == x.Id) && x.ToVersion == null);
                foreach (var attachment in eliminadosApprovable) attachment.Delete();

                var agregadosApprovable = lista.Where(x => !ListaAttachments.Any(y => y.Id == x.Id));
                foreach (var attachment in lista)
                {
                    attachment.Update(new AttachmentForUpdate
                    {
                        OwnerId = Id,
                        OwnerType = GetType().Name
                    });
                    ListaAttachments.Add(attachment);
                }
            }
        }
    }
}
