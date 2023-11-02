namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class EventOrganizerPermissions
{
    public static List<string> Provider = new()
    {
        Permissions.Students.View,
        
        Permissions.Events.View,
        Permissions.Events.Delete,
        Permissions.Events.Create,
        Permissions.Events.Edit,
        Permissions.Events.Cancel,
        Permissions.Events.ApproveRegistration,
        Permissions.Events.RejectRegistration,
        
        Permissions.EventCollaborationRequests.View,
    };
}