using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Exceptions;

public class EventCollaborationRequestApproveTimeExpiredException : ResourceInvalidDataException
{
    public EventCollaborationRequestApproveTimeExpiredException(Guid id) 
        : base($"Collaboration request '{id}' can't be approved before 1 day of event start time!", ErrorCodes.EventCollaborationRequestApproveTimeExpired)
    {
    }
}