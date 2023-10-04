using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class DuplicateStudentCitizenIdentifierException : ResourceAlreadyExistException
{
    public DuplicateStudentCitizenIdentifierException(string citizenIdentifier) 
        : base(nameof(Student), nameof(Student.CitizenId), citizenIdentifier, ErrorCodes.DuplicateStudentCitizenIdentifier)
    {
    }
}