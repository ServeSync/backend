using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

public class StudentDeletedDomainEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public string IdentityId { get; set; }
    
    public StudentDeletedDomainEvent(Guid id, string identityId)
    {
        Id = id;
        IdentityId = identityId;
    }
}