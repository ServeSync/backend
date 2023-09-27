using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

public record StudentCodeUpdatedDomainEvent : EquatableDomainEvent
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string IdentityId { get; set; }
    
    public StudentCodeUpdatedDomainEvent(Guid id, string code, string identityId)
    {
        Id = id;
        Code = code;
        IdentityId = identityId;
    }
}