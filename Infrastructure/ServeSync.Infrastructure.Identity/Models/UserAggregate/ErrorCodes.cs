namespace ServeSync.Infrastructure.Identity.Models.UserAggregate;

public static class ErrorCodes
{
    public const string UserNotFound = "User:000001";
    public const string UserNameOrEmailNotFound = "User:000002";
    public const string AccountLockedOut = "User:000003";
    public const string InvalidCredential = "User:000004";
}