using SharedKernel.Domain;

namespace Template.Domain.ExampleWithIntId.DomainEvents;

public sealed class ExampleWithIntIdUpdated : DomainEvent
{
    public int Id { get; set; }
}