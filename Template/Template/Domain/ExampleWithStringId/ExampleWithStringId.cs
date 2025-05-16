using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using Template.Domain.ExampleWithStringId.DomainEvents;
using Template.Domain.ExampleWithStringId.Models;
using Template.Domain.ExampleWithStringId.Rules;

namespace Template.Domain.ExampleWithStringId;

public class ExampleWithStringId : BaseEntity<string>
{
    // For EF + Mocking
    protected ExampleWithStringId()
    {
    }

    [Required] public string Code { get; set; }
    [Required] public string Name { get; set; }

    public static ExampleWithStringId Create(ExampleWithStringIdForCreation exampleWithStringIdForCreation)
    {
        CheckRule(new HasMaxLengthRule(exampleWithStringIdForCreation.Code, 8));
        CheckRule(new HasMaxLengthRule(exampleWithStringIdForCreation.Name, 50));

        var newExample = new ExampleWithStringId
        {
            Code = exampleWithStringIdForCreation.Code,
            Name = exampleWithStringIdForCreation.Name
        };

        newExample.QueueDomainEvent(new ExampleWithStringIdCreated { ExampleWithStringId = newExample });

        return newExample;
    }

    public ExampleWithStringId Update(ExampleWithStringIdForUpdate model)
    {
        QueueDomainEvent(new ExampleWithStringIdUpdated { Code = Code });
        return this;
    }
}