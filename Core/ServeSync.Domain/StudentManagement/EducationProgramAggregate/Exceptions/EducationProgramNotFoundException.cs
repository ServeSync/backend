using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.EducationProgramAggregate.Exceptions;

public class EducationProgramNotFoundException : ResourceNotFoundException
{
    public EducationProgramNotFoundException(Guid id) 
        : base(nameof(EducationProgram), id, ErrorCodes.EducationProgramNotFound)
    {
    }
}