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
}