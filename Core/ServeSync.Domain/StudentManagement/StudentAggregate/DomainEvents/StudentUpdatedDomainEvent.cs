using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

public record StudentUpdatedDomainEvent : EquatableDomainEvent
{
    public Guid Id { get; set; }
    
    public StudentUpdatedDomainEvent(Guid id)
    {
        Id = id;
    }
}