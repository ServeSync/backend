namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class StudentPermissions
{
    public static List<string> Provider = new()
    {
        Permissions.Students.View,
        
        Permissions.Students.ViewProfile,
        Permissions.Students.EditProfile,
        
        Permissions.Events.View,
        
        Permissions.Proofs.View,
        Permissions.Proofs.Create,
        Permissions.Proofs.Delete,
        Permissions.Proofs.Update
    };
}