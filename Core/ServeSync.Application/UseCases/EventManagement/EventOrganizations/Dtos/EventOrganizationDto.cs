using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? IdentityId { get; set; }
    public OrganizationStatus Status { get; set; }
    public int HostedEvents { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class EventOrganizationDetailDto : EventOrganizationDto
{
    public List<EventOrganizationContactDto> Contacts { get; set; } = null!;
}