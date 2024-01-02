using ServeSync.Application.Common;

namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class StudentPermissions
{
    public static List<string> Provider = new()
    {
        AppPermissions.Users.ViewProfile,
        
        AppPermissions.Students.View,
        
        AppPermissions.Students.ViewProfile,
        AppPermissions.Students.EditProfile,
        
        AppPermissions.Events.View,
        
        AppPermissions.Students.Export,
        
        AppPermissions.Proofs.View,
        AppPermissions.Proofs.Create,
        AppPermissions.Proofs.Delete,
        AppPermissions.Proofs.Update
    };
}