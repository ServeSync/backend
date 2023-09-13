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

        new(PermissionManagement.View, "View permissions."),
        new(PermissionManagement.GrantToRole, "Add permission to role."),
        new(PermissionManagement.GrantToUser, "Add permission to user."),
        new(PermissionManagement.UnGrantFromRole, "Remove permission from role."),
        new(PermissionManagement.UnGrantFromUser, "Remove permission from user."),
    };

    public static class Roles
    {
        public const string Group = $"{Default}.Roles";

        public const string View = $"{Group}.View";
        public const string Edit = $"{Group}.Edit";
        public const string Create = $"{Group}.Create";
        public const string Delete = $"{Group}.Delete";
    }

    public static class PermissionManagement
    {
        public const string Group = $"{Default}.PermissionManagement";

        public const string View = $"{Group}.View";
        public const string GrantToRole = $"{Group}.GrantToRole";
        public const string GrantToUser = $"{Group}.GrantToUser";
        public const string UnGrantFromRole = $"{Group}.UnGrantFromRole";
        public const string UnGrantFromUser = $"{Group}.UnGrantFromUser";
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