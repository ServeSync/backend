using ServeSync.Application.Common;

namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class EventOrganizationPermissions
{
    public static List<string> Provider = new()
    {
        AppPermissions.Users.ViewProfile,
        
        AppPermissions.Students.View,
            
        AppPermissions.Events.Management,
        AppPermissions.Events.View,
        AppPermissions.Events.Create,
        AppPermissions.Events.Edit,
        AppPermissions.Events.Cancel,
        AppPermissions.Events.ApproveRegistration,
        AppPermissions.Events.RejectRegistration,
            
        AppPermissions.EventOrganizations.View,
        AppPermissions.EventOrganizations.Update,
        AppPermissions.EventOrganizations.ViewContact,
        AppPermissions.EventOrganizations.UpdateContact,
        AppPermissions.EventOrganizations.RemoveContact,
        AppPermissions.EventOrganizations.AddContact
    };
}