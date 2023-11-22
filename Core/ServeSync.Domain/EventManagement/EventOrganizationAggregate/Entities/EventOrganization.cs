using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

public class EventOrganization : AuditableAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string? Address { get; private set; }
    public string ImageUrl { get; private set; }
    public string? IdentityId { get; private set; }
    public Guid? TenantId { get; private set; }
    public OrganizationStatus Status { get; private set; }
    public List<EventOrganizationContact> Contacts { get; private set; }
    public List<OrganizationInEvent> OrganizationInEvents { get; private set; } = new();
    
    internal EventOrganization(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address,
        OrganizationStatus status = OrganizationStatus.Pending)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        Email = Guard.NotNullOrEmpty(email, nameof(Email));
        PhoneNumber = Guard.NotNullOrEmpty(phoneNumber, nameof(PhoneNumber));
        ImageUrl = Guard.NotNullOrEmpty(imageUrl, nameof(ImageUrl));
        Description = description;
        Address = address;
        Status = OrganizationStatus.Pending;
        Contacts = new List<EventOrganizationContact>();

        if (Status == OrganizationStatus.Pending)
        {
            AddDomainEvent(new NewPendingEventOrganizationCreatedDomainEvent(this));    
        }
    }

    internal EventOrganizationContact AddContact(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        bool? gender, 
        DateTime? birth, 
        string? address, 
        string? position)
    {
        if (Status != OrganizationStatus.Active)
        {
            throw new EventOrganizationNotActiveException(Id);
        }
        
        if (Contacts.Any(x => x.Email == email))
        {
            throw new EventOrganizationContactAlreadyExistedException(Id, email);
        }
        
        var contact = new EventOrganizationContact(name, email, phoneNumber, imageUrl, Id, gender, birth, address, position);
        Contacts.Add(contact);

        return contact;
    }
    
    internal void Update(
        string name, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        PhoneNumber = Guard.NotNullOrEmpty(phoneNumber, nameof(PhoneNumber));
        ImageUrl = Guard.NotNullOrEmpty(imageUrl, nameof(ImageUrl));
        Description = description;
        Address = address;
        
        AddDomainEvent(new EventOrganizationUpdatedDomainEvent(this));
    }

    internal void UpdateEventOrganizationContact(
        Guid eventOrganizationContactId,
        string name,
        string phoneNumber,
        string imageUrl,
        bool? gender,
        DateTime? birth,
        string? address,
        string? position)
    {
        var eventOrganizationContact = Contacts.FirstOrDefault(x => x.Id == eventOrganizationContactId);
        if (eventOrganizationContact == null)
        {
            throw new EventOrganizationContactNotFoundException(eventOrganizationContactId);
        }
        
        eventOrganizationContact.Update(name, phoneNumber, imageUrl, gender, birth, address, position);
    }
    
    internal void DeleteEventOrganizationContact(Guid eventOrganizationContactId)
    {
        var eventOrganizationContact = Contacts.FirstOrDefault(x => x.Id == eventOrganizationContactId);
        if (eventOrganizationContact == null)
        {
            throw new EventOrganizationContactNotFoundException(eventOrganizationContactId);
        }
        
        Contacts.Remove(eventOrganizationContact);
        AddDomainEvent(new EventOrganizationContactDeletedDomainEvent(eventOrganizationContactId, eventOrganizationContact.IdentityId, TenantId.GetValueOrDefault()));
    }

    public void ApproveInvitation()
    {
        if (Status != OrganizationStatus.Pending)
        {
            throw new EventOrganizationNotPendingException(Id);
        }
        
        Status = OrganizationStatus.Active;
        AddDomainEvent(new EventOrganizationInvitationApprovedDomainEvent(this));
    }
    
    public void RejectInvitation()
    {
        if (Status != OrganizationStatus.Pending)
        {
            throw new EventOrganizationNotPendingException(Id);
        }
        
        Status = OrganizationStatus.Rejected;
    }
    
    public void ApproveContactInvitation(Guid eventOrganizationContactId)
    {
        var eventOrganizationContact = Contacts.FirstOrDefault(x => x.Id == eventOrganizationContactId);
        if (eventOrganizationContact == null)
        {
            throw new EventOrganizationContactNotFoundException(eventOrganizationContactId);
        }
        
        eventOrganizationContact.ApproveInvitation();
        AddDomainEvent(new OrganizationContactInvitationApprovedDomainEvent(eventOrganizationContact, this));
    }
    
    public void RejectContactInvitation(Guid eventOrganizationContactId)
    {
        var eventOrganizationContact = Contacts.FirstOrDefault(x => x.Id == eventOrganizationContactId);
        if (eventOrganizationContact == null)
        {
            throw new EventOrganizationContactNotFoundException(eventOrganizationContactId);
        }
        
        eventOrganizationContact.RejectInvitation();
    }
    
    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
    
    public void SetTenantId(Guid tenantId)
    {
        TenantId = tenantId;
    }

    private EventOrganization()
    {
        Contacts = new List<EventOrganizationContact>();
    }
    
}