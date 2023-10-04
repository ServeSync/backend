using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class DuplicateStudentEmailException : ResourceAlreadyExistException
{
    public DuplicateStudentEmailException(string email) 
        : base(nameof(Student), nameof(Student.Email), email, ErrorCodes.DuplicateStudentEmail)
    {
    }
}