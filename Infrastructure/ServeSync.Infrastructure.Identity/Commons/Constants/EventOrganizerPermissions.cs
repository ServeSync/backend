using ServeSync.Application.Common;

namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class EventOrganizerPermissions
{
    public static List<string> Provider = new()
    {
        AppPermissions.Users.ViewProfile,
        
        AppPermissions.Events.Management,
        AppPermissions.Events.View,
        
        AppPermissions.EventOrganizations.View,
        AppPermissions.EventOrganizations.ViewContact,
        AppPermissions.EventOrganizations.UpdateContact,
    };
}