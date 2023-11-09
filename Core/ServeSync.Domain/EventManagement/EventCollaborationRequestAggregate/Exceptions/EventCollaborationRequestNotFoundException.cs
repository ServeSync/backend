using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Exceptions;

public class EventCollaborationRequestNotFoundException : ResourceNotFoundException
{
    public EventCollaborationRequestNotFoundException(Guid id) : base(nameof(EventCollaborationRequest), id, ErrorCodes.EventCollaborationRequestNotFound)
    {
    }
}