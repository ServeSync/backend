using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
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
    public List<EventOrganizationContact> Contacts { get; private set; }
    public List<OrganizationInEvent> OrganizationInEvents { get; private set; } = new();
    
    internal EventOrganization(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        Email = Guard.NotNullOrEmpty(email, nameof(Email));
        PhoneNumber = Guard.NotNullOrEmpty(phoneNumber, nameof(PhoneNumber));
        ImageUrl = Guard.NotNullOrEmpty(imageUrl, nameof(ImageUrl));
        Description = description;
        Address = address;
        Contacts = new List<EventOrganizationContact>();
        
        AddDomainEvent(new NewEventOrganizationCreatedDomainEvent(this));
    }

    internal void AddContact(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        bool? gender, 
        DateTime? birth, 
        string? address, 
        string? position)
    {
        if (Contacts.Any(x => x.Email == email))
        {
            throw new EventOrganizationContactAlreadyExistedException(Id, email);
        }
        
        var contact = new EventOrganizationContact(name, email, phoneNumber, imageUrl, Id, gender, birth, address, position);
        Contacts.Add(contact);
    }
    
    internal void Update(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        Email = Guard.NotNullOrEmpty(email, nameof(Email));
        PhoneNumber = Guard.NotNullOrEmpty(phoneNumber, nameof(PhoneNumber));
        ImageUrl = Guard.NotNullOrEmpty(imageUrl, nameof(ImageUrl));
        Description = description;
        Address = address;
    }
    
    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }

    private EventOrganization()
    {
        Contacts = new List<EventOrganizationContact>();
    }
    
}