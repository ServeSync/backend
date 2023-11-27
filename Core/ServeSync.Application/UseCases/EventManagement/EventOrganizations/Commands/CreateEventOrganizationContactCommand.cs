using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class CreateEventOrganizationContactCommand : ICommand<Guid>
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }
    public DateTime? Birth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool? Gender { get; set; }
    public string? Address { get; set; }
    public string ImageUrl { get; set; }
    public string? Position { get; set; }

    public CreateEventOrganizationContactCommand(Guid organizationId, EventOrganizationContactCreateDto dto)
    {
        OrganizationId = organizationId;
        Name = dto.Name;
        Birth = dto.Birth;
        Email = dto.Email;
        PhoneNumber = dto.PhoneNumber;
        Gender = dto.Gender;
        Address = dto.Address;
        ImageUrl = dto.ImageUrl;
        Position = dto.Position;
    }
}