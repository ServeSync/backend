using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class RejectEventCommand : ICommand
{
    public Guid EventId { get; set; }
    
    public RejectEventCommand(Guid eventId)
    {
        EventId = eventId;
    }
}