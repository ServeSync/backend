using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

public class ProofNotPendingException : ResourceInvalidDataException
{
    public ProofNotPendingException(Guid id) 
        : base($"Proof with '{id}' is not in pending status!", ErrorCodes.ProofNotPending)
    {
    }
}