using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class CreateEventCommand : ICommand<Guid>
{
    public EventCreateDto Event { get; set; }
    
    public CreateEventCommand(EventCreateDto eventCreateDto)
    {
        Event = eventCreateDto;
    }
}