using SharedKernel.Domain;

namespace Template.Domain.ExampleWithStringId.DomainEvents;

public sealed class ExampleWithStringIdUpdated : DomainEvent
{
    public string Code { get; set; }
}