using ServeSync.Application.Common;

namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class StudentAffairPermissions
{
    public static List<string> Provider = new()
    {
        AppPermissions.Roles.Management,
        AppPermissions.Roles.Create,
        AppPermissions.Roles.View,
        AppPermissions.Roles.Edit,
        AppPermissions.Roles.Delete,
        AppPermissions.Roles.ViewPermission,
        AppPermissions.Roles.UpdatePermission,

        AppPermissions.PermissionManagement.View,
        
        AppPermissions.Users.ViewPermissions,
        AppPermissions.Users.ViewProfile,
        AppPermissions.Users.EditRoles,
        AppPermissions.Users.Management,
        AppPermissions.Users.ViewRoles,
        
        AppPermissions.Students.Management,
        AppPermissions.Students.View,
        AppPermissions.Students.Delete,
        AppPermissions.Students.Create,
        AppPermissions.Students.Edit,
        AppPermissions.Students.ViewProfile,
        AppPermissions.Students.EditProfile, 
        AppPermissions.Students.Import,
        AppPermissions.Students.Export,
        
        AppPermissions.Events.Management,
        AppPermissions.Events.View,
        AppPermissions.Events.Delete,
        AppPermissions.Events.Create,
        AppPermissions.Events.Edit,
        AppPermissions.Events.Approve,
        AppPermissions.Events.Reject,
        AppPermissions.Events.Cancel,
        AppPermissions.Events.ApproveRegistration,
        AppPermissions.Events.RejectRegistration,
        
        AppPermissions.EventOrganizations.Management,
        AppPermissions.EventOrganizations.View,
        AppPermissions.EventOrganizations.Create,
        AppPermissions.EventOrganizations.Update,
        AppPermissions.EventOrganizations.Delete,
        AppPermissions.EventOrganizations.ViewContact,
        AppPermissions.EventOrganizations.UpdateContact,
        AppPermissions.EventOrganizations.RemoveContact,
        AppPermissions.EventOrganizations.AddContact,
        
        AppPermissions.EventCollaborationRequests.Management,
        AppPermissions.EventCollaborationRequests.View,
        AppPermissions.EventCollaborationRequests.Approve,
        AppPermissions.EventCollaborationRequests.Reject,
        
        AppPermissions.Proofs.Management,
        AppPermissions.Proofs.View,
        AppPermissions.Proofs.Create,
        AppPermissions.Proofs.Delete,
        AppPermissions.Proofs.Update,
        AppPermissions.Proofs.Approve,
        AppPermissions.Proofs.Reject,
    };
}