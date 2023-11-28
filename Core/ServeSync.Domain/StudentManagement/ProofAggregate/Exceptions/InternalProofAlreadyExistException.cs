using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;
 
public class InternalProofAlreadyExistException : ResourceAlreadyExistException
{
    public InternalProofAlreadyExistException(Guid eventId, Guid studentId) 
        : base($"Internal proof for event {eventId} and student {studentId} already exist!", ErrorCodes.InternalProofAlreadyExist)
    {
    }
}