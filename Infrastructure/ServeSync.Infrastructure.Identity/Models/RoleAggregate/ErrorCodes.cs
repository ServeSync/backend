namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate;

public static class ErrorCodes
{
    public const string RoleNotFound = "Role:000001";
    public const string PermissionHasAlreadyGrantedToRole = "Role:000002";
    public const string PermissionHasNotGrantedToRole = "Role:000003";
    public const string DefaultRoleAccessDenied = "Role:000004";
}