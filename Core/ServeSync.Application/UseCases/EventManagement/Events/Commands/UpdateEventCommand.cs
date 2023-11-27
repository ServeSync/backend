using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class UpdateEventCommand : ICommand
{
    public Guid Id { get; set; }
    public EventUpdateDto Event { get; set; }
    
    public UpdateEventCommand(Guid id, EventUpdateDto eventUpdateDto)
    {
        Id = id;
        Event = eventUpdateDto;
    }

    public bool IsUpdateBasicInfo()
    {
        return !string.IsNullOrEmpty(Event.Name) &&
               !string.IsNullOrEmpty(Event.Introduction) &&
               !string.IsNullOrEmpty(Event.Description) &&
               !string.IsNullOrEmpty(Event.ImageUrl) &&
               Event is { StartAt: not null, EndAt: not null } &&
               Event.Type.HasValue &&
               Event.ActivityId.HasValue &&
               Event.RepresentativeOrganizationId.HasValue &&
               Event.Address != null;

    }
}