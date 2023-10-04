using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.FacultyAggregate.Exceptions;

public class FacultyNotFoundException : ResourceNotFoundException
{
    public FacultyNotFoundException(Guid id) 
        : base(nameof(Faculty), id, ErrorCodes.FacultyNotFound)
    {
    }
}