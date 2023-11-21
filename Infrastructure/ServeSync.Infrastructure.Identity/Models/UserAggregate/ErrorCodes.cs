namespace ServeSync.Infrastructure.Identity.Models.UserAggregate;

public static class ErrorCodes
{
    public const string UserNotFound = "User:000001";
    public const string UserNameOrEmailNotFound = "User:000002";
    public const string AccountLockedOut = "User:000003";
    public const string InvalidCredential = "User:000004";
    public const string RefreshTokenAlreadyExpire = "User:000005";
    public const string RefreshTokenNotFound = "User:000006";
    public const string RefreshTokenAlreadyAdded = "User:000007";
    public const string AccessTokenStillValid = "User:000008";
    public const string UserAlreadyInTenant = "User:000009";
    public const string UserNotInTenant = "User:000010";
    public const string InvalidForgetPasswordToken = "InvalidToken";
}