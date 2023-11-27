using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationContactDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool? Gender { get; set; }
    public DateTime? Birth { get; set; }
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? Position { get; set; }
    public OrganizationStatus Status { get; set; }
    public Guid EventOrganizationId { get; set; }
}