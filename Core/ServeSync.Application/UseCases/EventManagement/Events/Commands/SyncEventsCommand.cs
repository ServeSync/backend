using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class SyncEventsCommand : ICommand
{
    public Guid[] EventIds { get; set; }
    
    public SyncEventsCommand(Guid[] eventIds)
    {
        EventIds = eventIds;
    }
}