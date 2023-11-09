using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class ApproveEventCommand :ICommand<Guid>
{
    public Guid EventId { get; set; }

    public ApproveEventCommand(Guid eventId)
    {
        EventId = eventId;
    }
}