using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class CancelEventCommand : ICommand<Guid>
{
    public Guid EventId { get; set; }

    public CancelEventCommand(Guid eventId)
    {
        EventId = eventId;
    }
}