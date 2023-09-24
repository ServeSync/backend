using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.EducationProgramAggregate.Exceptions;

public class DuplicateEducationProgramException : ResourceAlreadyExistException
{
    public DuplicateEducationProgramException(string name) 
        : base("Education Program", nameof(EducationProgram.Name), name, ErrorCodes.DuplicateEducationProgram)
    {
    }
}