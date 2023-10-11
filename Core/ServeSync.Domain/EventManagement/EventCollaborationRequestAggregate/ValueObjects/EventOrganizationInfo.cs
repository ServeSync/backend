using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.ValueObjects;

public record EventOrganizationInfo : ValueObject
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string? Address { get; private set; }
    public string ImageUrl { get; private set; }

    internal EventOrganizationInfo(
        string name, 
        string? description, 
        string email, 
        string phoneNumber, 
        string? address,
        string imageUrl)
    {
        Name = Guard.NotNullOrWhiteSpace(name, nameof(Name));
        Description = description;
        Email = Guard.NotNullOrWhiteSpace(email, nameof(Email));
        PhoneNumber = Guard.NotNullOrWhiteSpace(phoneNumber, nameof(PhoneNumber));
        Address = address;
        ImageUrl = Guard.NotNullOrWhiteSpace(imageUrl, nameof(ImageUrl));
    }
}