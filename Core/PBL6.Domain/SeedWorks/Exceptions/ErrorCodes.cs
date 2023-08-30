namespace PBL6.Domain.SeedWorks.Exceptions;

public static class ErrorCodes
{
    // Resource
    public const string ResourceNotFound = "Resource:100";
    public const string ResourceAlreadyExist = "Resource:101";
    public const string ResourceAccessDenied = "Resource:103";
    public const string ResourceInvalidOperation = "Resource:104";
    public const string ResourceInvalidData = "Resource:105";
    
    // System
    public const string SystemError = "500";
}