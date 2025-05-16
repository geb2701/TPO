using SharedKernel.Domain;

namespace Template.Domain.ExampleWithStringId.DomainEvents;

public sealed class ExampleWithStringIdCreated : DomainEvent
{
    public ExampleWithStringId ExampleWithStringId { get; set; }
}