using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Exceptions;

public class EventCollaborationRequestCanNotBeRejectedException : ResourceInvalidDataException
{
    public EventCollaborationRequestCanNotBeRejectedException(Guid id) 
        : base($"Collaboration request '{id}' can't be rejected!", ErrorCodes.EventCollaborationRequestCanNotBeRejected)
    {
    }
}