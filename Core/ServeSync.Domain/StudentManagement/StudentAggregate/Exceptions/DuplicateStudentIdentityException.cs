using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class DuplicateStudentIdentityException : ResourceAlreadyExistException
{
    public DuplicateStudentIdentityException(string identityId) 
        : base(nameof(Student), nameof(Student.IdentityId), identityId, ErrorCodes.DuplicateStudentIdentity)
    {
    }
}