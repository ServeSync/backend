using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class DuplicateStudentCodeException : ResourceAlreadyExistException
{
    public DuplicateStudentCodeException(string code) 
        : base(nameof(Student), nameof(Student.Code), code, ErrorCodes.DuplicateStudentCode)
    {
    }
}