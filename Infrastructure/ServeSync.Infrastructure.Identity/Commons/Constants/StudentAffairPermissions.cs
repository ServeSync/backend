namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class StudentAffairPermissions
{
    public static List<string> Provider = new()
    {
        Permissions.Roles.Management,
        Permissions.Roles.Create,
        Permissions.Roles.View,
        Permissions.Roles.Edit,
        Permissions.Roles.Delete,
        Permissions.Roles.ViewPermission,
        Permissions.Roles.UpdatePermission,

        Permissions.PermissionManagement.View,
        
        Permissions.Users.ViewPermissions,
        Permissions.Users.ViewProfile,
        
        Permissions.Students.Management,
        Permissions.Students.View,
        Permissions.Students.Delete,
        Permissions.Students.Create,
        Permissions.Students.Edit,
        Permissions.Students.ViewProfile,
        Permissions.Students.EditProfile, 
        Permissions.Students.Import,
        Permissions.Students.Export,
        
        Permissions.Events.Management,
        Permissions.Events.View,
        Permissions.Events.Delete,
        Permissions.Events.Create,
        Permissions.Events.Edit,
        Permissions.Events.Approve,
        Permissions.Events.Reject,
        Permissions.Events.Cancel,
        Permissions.Events.ApproveRegistration,
        Permissions.Events.RejectRegistration,
        
        Permissions.EventOrganizations.Management,
        Permissions.EventOrganizations.View,
        Permissions.EventOrganizations.Create,
        Permissions.EventOrganizations.Update,
        Permissions.EventOrganizations.Delete,
        Permissions.EventOrganizations.ViewContact,
        Permissions.EventOrganizations.UpdateContact,
        Permissions.EventOrganizations.RemoveContact,
        Permissions.EventOrganizations.AddContact,
        
        Permissions.EventCollaborationRequests.Management,
        Permissions.EventCollaborationRequests.View,
        Permissions.EventCollaborationRequests.Approve,
        Permissions.EventCollaborationRequests.Reject,
        
        Permissions.Proofs.Management,
        Permissions.Proofs.View,
        Permissions.Proofs.Create,
        Permissions.Proofs.Delete,
        Permissions.Proofs.Update,
        Permissions.Proofs.Approve,
        Permissions.Proofs.Reject,
    };
}