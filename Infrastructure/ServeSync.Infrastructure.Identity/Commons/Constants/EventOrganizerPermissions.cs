namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class EventOrganizerPermissions
{
    public static List<string> Provider = new()
    {
        Permissions.Users.ViewProfile,
        
        Permissions.Events.Management,
        Permissions.Events.View,
        
        Permissions.EventOrganizations.View,
        Permissions.EventOrganizations.ViewContact,
        Permissions.EventOrganizations.UpdateContact,
    };
}