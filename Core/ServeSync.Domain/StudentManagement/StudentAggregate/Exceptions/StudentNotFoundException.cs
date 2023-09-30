using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class StudentNotFoundException : ResourceNotFoundException
{
    public StudentNotFoundException(Guid id) : base(nameof(Student), id, ErrorCodes.StudentNotFound)
    {
    }
    
    public StudentNotFoundException(string id) : base(nameof(Student), nameof(Student.IdentityId),id, ErrorCodes.StudentNotFound)
    {
    }
}