using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

public class ProofCanNotBeRejectedException : ResourceInvalidDataException
{
    public ProofCanNotBeRejectedException(Guid id) 
        : base($"Proof '{id}' can't be rejected!", ErrorCodes.ProofCanNotBeRejected)
    {
    }
}