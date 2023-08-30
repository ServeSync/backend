namespace PBL6.Domain.SeedWorks.Exceptions.Resources;

public class ResourceInvalidOperationException : CoreException
{
    public ResourceInvalidOperationException(string message) : base(message, ErrorCodes.ResourceInvalidOperation)
    {
    }

    public ResourceInvalidOperationException(string message, string errorCode) : base(message, errorCode)
    {
    }
}