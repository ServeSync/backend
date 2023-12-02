using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

public class ProofCanNotBeDeletedException :  ResourceInvalidDataException
{
    public ProofCanNotBeDeletedException(Guid id) 
        : base($"Proof '{id}' can't be deleted!", ErrorCodes.ProofCanNotBeDeleted)
    {
    }
}
