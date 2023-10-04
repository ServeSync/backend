using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

public record StudentContactInfoUpdatedDomainEvent : EquatableDomainEvent
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string ImageUrl { get; set; }
    public string IdentityId { get; set; }
    
    public StudentContactInfoUpdatedDomainEvent(Guid id, string fullName, string email, string imageUrl, string identityId)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        ImageUrl = imageUrl;
        IdentityId = identityId;
    }
}