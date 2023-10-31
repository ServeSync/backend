using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Shared;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;

public class EventCollaborationRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public string ImageUrl { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public EventType Type { get; set; }
    public EventAddressDto Address { get; set; } = null!;
    public EventOrganizationInfoDto Organization { get; set; } = null!;
}