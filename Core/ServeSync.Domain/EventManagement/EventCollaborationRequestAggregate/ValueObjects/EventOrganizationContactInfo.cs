using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.ValueObjects;

public record EventOrganizationContactInfo : ValueObject
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public bool? Gender { get; private set; }
    public string? Address { get; private set; }
    public DateTime? Birth { get; private set; }
    public string? Position { get; private set; }
    public string ImageUrl { get; private set; }

    internal EventOrganizationContactInfo(
        string name, 
        string email, 
        string phoneNumber, 
        bool? gender, 
        string? address,
        DateTime? birth, 
        string? position, 
        string imageUrl)
    {
        Name = Guard.NotNullOrWhiteSpace(name, nameof(Name));
        Email = Guard.NotNullOrWhiteSpace(email, nameof(Email));
        PhoneNumber = Guard.NotNullOrWhiteSpace(phoneNumber, nameof(PhoneNumber));
        Gender = gender;
        Address = address;
        Birth = birth;
        Position = position;
        ImageUrl = Guard.NotNullOrWhiteSpace(imageUrl, nameof(ImageUrl));;
    }
}