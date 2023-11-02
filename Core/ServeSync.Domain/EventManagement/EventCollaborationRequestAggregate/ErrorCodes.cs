namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate;

public static class ErrorCodes
{
    public const string EventCollaborationRequestNotFound = "EventCollaborationRequest:000001";
    public const string EventCollaborationRequestApproveTimeExpired = "EventCollaborationRequest:000002";
    public const string EventCollaborationRequestNotPending = "EventCollaborationRequest:000003";
}