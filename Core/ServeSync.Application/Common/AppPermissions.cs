namespace ServeSync.Application.Common;

public static class AppPermissions
{
    public const string Default = "ServeSync.Permissions";

    public static class Roles
    {
        public const string Group = $"{Default}.Roles";
        
        public const string Management = $"{Group}.Management";

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
        public const string Group = $"{Default}.Users";

        public const string Management = $"{Group}.Management";
        public const string ViewProfile = $"{Group}.ViewProfile";
        public const string ViewPermissions = $"{Group}.ViewPermissions";
        public const string View = $"{Group}.View";
        public const string ViewRoles = $"{Group}.ViewRoles";
        public const string EditRoles = $"{Group}.EditRoles";
    }
    
    public static class Students
    {
        public const string Group = $"{Default}.Students";

        public const string Management = $"{Group}.Management";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Delete = $"{Group}.Delete";
        public const string Edit = $"{Group}.Edit";
        
        public const string ViewProfile = $"{Group}.ViewProfile";
        public const string EditProfile = $"{Group}.EditProfile";
        
        public const string Import = $"{Group}.Import";
        public const string Export = $"{Group}.Export";
    }
    
    public static class Events
    {
        public const string Group = $"{Default}.Events";
        
        public const string Management = $"{Group}.Management";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Delete = $"{Group}.Delete";
        public const string Edit = $"{Group}.Edit";
        
        public const string Approve = $"{Group}.Approve";
        public const string Cancel = $"{Group}.Cancel";
        public const string Reject = $"{Group}.Reject";

        public const string ApproveRegistration = $"{Group}.ApproveRegistration";
        public const string RejectRegistration = $"{Group}.RejectRegistration";
    }

    public static class EventCollaborationRequests
    {
        public const string Group = $"{Default}.EventCollaborationRequests";
        
        public const string Management = $"{Group}.Management";
        public const string View = $"{Group}.View";
        public const string Approve = $"{Group}.Approve";
        public const string Reject = $"{Group}.Reject";
    }

    public static class EventOrganizations
    {
        public const string Group = $"{Default}.EventOrganizations";
        
        public const string Management = $"{Group}.Management";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Update = $"{Group}.Update";
        public const string Delete = $"{Group}.Delete";
        
        public const string AddContact = $"{Group}.AddContact";
        public const string ViewContact = $"{Group}.ViewContact";
        public const string RemoveContact = $"{Group}.RemoveContact";
        public const string UpdateContact = $"{Group}.UpdateContact";
    }

    public static class Proofs
    {
        public const string Group = $"{Default}.Proofs";
        
        public const string Management = $"{Group}.Management";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Update = $"{Group}.Update";
        public const string Delete = $"{Group}.Delete";
        
        public const string Approve = $"{Group}.Approve";
        public const string Reject = $"{Group}.Reject";
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