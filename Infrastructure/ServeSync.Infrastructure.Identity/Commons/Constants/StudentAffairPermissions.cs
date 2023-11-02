namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class StudentAffairPermissions
{
    public static List<string> Provider = new()
    {
        Permissions.Roles.Create,
        Permissions.Roles.View,
        Permissions.Roles.Edit,
        Permissions.Roles.Delete,
        Permissions.Roles.ViewPermission,
        Permissions.Roles.UpdatePermission,

        Permissions.PermissionManagement.View,
        
        Permissions.Users.ViewPermissions,
        Permissions.Users.ViewProfile,
        
        Permissions.Faculties.View,
        Permissions.HomeRooms.View,
        Permissions.EducationPrograms.View,
        
        Permissions.Students.View,
        Permissions.Students.Delete,
        Permissions.Students.Create,
        Permissions.Students.Edit,
        Permissions.Students.ViewProfile,
        Permissions.Students.EditProfile, 
        
        Permissions.Events.View,
        Permissions.Events.Delete,
        Permissions.Events.Create,
        Permissions.Events.Edit,
        Permissions.Events.Approve,
        Permissions.Events.Cancel,
        Permissions.Events.ApproveRegistration,
        Permissions.Events.RejectRegistration,
        
        Permissions.EventCollaborationRequests.View,
        Permissions.EventCollaborationRequests.Approve
    };
}