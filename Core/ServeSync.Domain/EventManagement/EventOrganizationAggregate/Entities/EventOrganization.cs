using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

public class EventOrganization : AggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string? Address { get; private set; }
    public string ImageUrl { get; private set; }
    public List<EventOrganizationContact> Contacts { get; private set; }
    
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
    }

    private EventOrganization()
    {
        Contacts = new List<EventOrganizationContact>();
    }
}