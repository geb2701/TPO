using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using Template.Domain.ExampleWithIntId.DomainEvents;
using Template.Domain.ExampleWithIntId.Models;
using Template.Domain.ExampleWithIntId.Rules;

namespace Template.Domain.ExampleWithIntId;

public class ExampleWithIntId : BaseEntity<int>
{
    // For EF + Mocking
    protected ExampleWithIntId()
    {
    }

    [Required] public string Name { get; set; }

    public static ExampleWithIntId Create(ExampleWithIntIdForCreation exampleWithStringIdForCreation)
    {
        CheckRule(new HasMaxLengthRule(exampleWithStringIdForCreation.Name, 50));

        var newExample = new ExampleWithIntId
        {
            Name = exampleWithStringIdForCreation.Name
        };

        newExample.QueueDomainEvent(new ExampleWithIntIdCreated { ExampleWithIntId = newExample });

        return newExample;
    }

    public ExampleWithIntId Update(ExampleWithIntIdForUpdate model)
    {
        QueueDomainEvent(new ExampleWithIntIdUpdated { Id = Id });
        return this;
    }
}