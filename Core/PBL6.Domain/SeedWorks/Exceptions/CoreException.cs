namespace PBL6.Domain.SeedWorks.Exceptions;

public class CoreException: Exception
{
    public string ErrorCode { get; set; }
    
    public CoreException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}