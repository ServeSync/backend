using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

public class EventOrganizationContact : Entity
{
    public string Name { get; private set; }
    public bool? Gender { get; private set; }
    public DateTime? Birth { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string? Address { get; private set; }
    public string ImageUrl { get; private set; }
    public string? Position { get; private set; }
    public OrganizationStatus Status { get; private set; }
    
    public Guid EventOrganizationId { get; private set; }
    public EventOrganization? EventOrganization { get; private set; }

    public string IdentityId { get; private set; } = null!;

    internal EventOrganizationContact(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        Guid eventOrganizationId, 
        bool? gender, 
        DateTime? birth, 
        string? address, 
        string? position,
        OrganizationStatus status = OrganizationStatus.Pending)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        Email = Guard.NotNullOrEmpty(email, nameof(Email));
        PhoneNumber = Guard.NotNullOrEmpty(phoneNumber, nameof(PhoneNumber));
        ImageUrl = Guard.NotNullOrEmpty(imageUrl, nameof(ImageUrl));
        EventOrganizationId = Guard.NotNull(eventOrganizationId, nameof(EventOrganizationId));
        Gender = gender;
        Birth = birth;
        Address = address;
        Position = position;
        Status = status;

        if (Status == OrganizationStatus.Pending)
        {
            AddDomainEvent(new NewPendingOrganizationContactCreatedDomainEvent(this));    
        }
    }

    internal void Update(
        string name,
        string phoneNumber,
        string imageUrl,
        bool? gender,
        DateTime? birth,
        string? address,
        string? position)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        Gender = gender;
        Birth = birth;
        PhoneNumber = Guard.NotNullOrEmpty(phoneNumber, nameof(PhoneNumber));
        Address = address;
        ImageUrl = Guard.NotNullOrEmpty(imageUrl, nameof(ImageUrl));
        Position = position;
    }
    
    public void ApproveInvitation()
    {
        if (Status != OrganizationStatus.Pending)
        {
            throw new EventOrganizationContactNotPendingException(Id);
        }
        
        Status = OrganizationStatus.Active;
    }
    
    public void RejectInvitation()
    {
        if (Status != OrganizationStatus.Pending)
        {
            throw new EventOrganizationContactNotPendingException(Id);
        }
        
        Status = OrganizationStatus.Rejected;
    }
    
    public void SetIdentityId(string identityId)
    {
        IdentityId = Guard.NotNullOrEmpty(identityId, nameof(IdentityId));
    }

    private EventOrganizationContact()
    {
        
    }
}