namespace ServeSync.Domain.SeedWorks.Exceptions.Resources;

public class ResourceInvalidDataException : CoreException
{
    public ResourceInvalidDataException(string message) : base(message, ErrorCodes.ResourceInvalidData)
    {
    }
}