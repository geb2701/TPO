namespace Template.Domain.Attachments.Abstractions
{
    public interface IAttachmentsEntity
    {
        ICollection<Attachment> ListaAttachments { get; set; }
        void SetAttachments(List<Attachment> lista);
        void DeleteAttachments();
    }
}
