namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class EventOrganizerPermissions
{
    public static List<string> Provider = new()
    {
        Permissions.Users.ViewProfile,
            
        Permissions.Students.View,
        
        Permissions.Events.View,
        
        Permissions.EventOrganizations.View,
        
        Permissions.EventCollaborationRequests.View,
    };
}