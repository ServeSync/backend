using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class Permissions
{
    public const string Default = "ServeSync.Permissions";

    public static List<ApplicationPermission> Provider => new()
    {
        new(Roles.Create, "Create role."),
        new(Roles.View, "View roles."),
        new(Roles.Edit, "Edit roles."),
        new(Roles.Delete, "Delete roles."),
        new(Roles.ViewPermission, "View permission of role."),
        new(Roles.UpdatePermission, "Update permission for role."),

        new(PermissionManagement.View, "View permissions."),
        
        new(Users.ViewPermissions, "View permissions of user."),
        new(Users.ViewProfile, "View profile of user."),
    };

    public static class Roles
    {
        public const string Group = $"{Default}.Roles";

        public const string View = $"{Group}.View";
        public const string Edit = $"{Group}.Edit";
        public const string Create = $"{Group}.Create";
        public const string Delete = $"{Group}.Delete";

        public const string ViewPermission = $"{Group}.ViewPermissions";
        public const string UpdatePermission = $"{Group}.UpdatePermissions";
    }

    public static class PermissionManagement
    {
        public const string Group = $"{Default}.PermissionManagement";

        public const string View = $"{Group}.View";
    }

    public static class Users
    {
        public const string Group = $"{Default}.PermissionManagement";

        public const string ViewProfile = $"{Group}.ViewProfile";
        public const string ViewPermissions = $"{Group}.ViewPermissions";
    }

    public static List<string> GeneratePermissionsForModule(string module)
    {
        return new List<string>()
        {
            $"{Default}.{module}.Create",
            $"{Default}.{module}.View",
            $"{Default}.{module}.Edit",
            $"{Default}.{module}.Delete",
        };
    }
}