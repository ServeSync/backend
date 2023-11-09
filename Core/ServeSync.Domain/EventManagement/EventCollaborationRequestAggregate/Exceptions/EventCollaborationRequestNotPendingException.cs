using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Exceptions;

public class EventCollaborationRequestNotPendingException : ResourceInvalidOperationException
{
    public EventCollaborationRequestNotPendingException(Guid id) 
        : base($"Collaboration request {id} is not in pending status!", ErrorCodes.EventCollaborationRequestNotPending)
    {
    }
}