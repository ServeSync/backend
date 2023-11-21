using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class UpdateEventOrganizationContactCommand : ICommand
{
    public Guid OrganizationId { get; set; }
    
    public Guid OrganizationContactId { get; set; }
    
    public string Name { get; set; }
    
    public bool? Gender { get; set; }
    
    public DateTime? Birth { get; set; }

    public string PhoneNumber { get; set; }
    
    public string? Address { get; set; }

    public string ImageUrl { get; set; }
    
    public string? Position { get; set; }

    public UpdateEventOrganizationContactCommand(
        Guid organizationId,
        Guid organizationContactId,
        string name,
        bool? gender,
        DateTime? birth,
        string phoneNumber,
        string? address,
        string imageUrl,
        string? position)
    {
        OrganizationId = organizationId;
        OrganizationContactId = organizationContactId;
        Name = name;
        Gender = gender;
        Birth = birth;
        PhoneNumber = phoneNumber;
        Address = address;
        ImageUrl = imageUrl;
        Position = position;
    }
    
}