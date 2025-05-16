using SharedKernel.Domain;

namespace Template.Domain.ExampleWithIntId.DomainEvents;

public sealed class ExampleWithIntIdCreated : DomainEvent
{
    public ExampleWithIntId ExampleWithIntId { get; set; }
}