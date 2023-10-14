using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

public class BasicEventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Introduction { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    
    public DateTime StartAt { get; init; }
    public DateTime EndAt { get; set; }
    
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public EventAddressDto Address { get; set; } = null!;
}

public class FlatEventDto : BasicEventDto
{
    public int Capacity { get; set; }
    public int Registered { get; set; }
    public int Rating { get; set; }

    public OrganizationInEventDto RepresentativeOrganization { get; set; } = null!;
}

public class OrganizationInEventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
}