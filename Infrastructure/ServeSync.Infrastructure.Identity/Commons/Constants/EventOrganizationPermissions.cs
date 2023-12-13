namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class EventOrganizationPermissions
{
    public static List<string> Provider = new()
    {
        Permissions.Users.ViewProfile,
        
        Permissions.Students.View,
            
        Permissions.Events.Management,
        Permissions.Events.View,
        Permissions.Events.Create,
        Permissions.Events.Edit,
        Permissions.Events.Cancel,
        Permissions.Events.ApproveRegistration,
        Permissions.Events.RejectRegistration,
            
        Permissions.EventOrganizations.View,
        Permissions.EventOrganizations.Update,
        Permissions.EventOrganizations.ViewContact,
        Permissions.EventOrganizations.UpdateContact,
        Permissions.EventOrganizations.RemoveContact,
        Permissions.EventOrganizations.AddContact
    };
}