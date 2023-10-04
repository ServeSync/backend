using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.FacultyAggregate.Exceptions;

public class DuplicateFacultyException : ResourceAlreadyExistException
{
    public DuplicateFacultyException(string name) 
        : base("Faculty", nameof(EducationProgram.Name), name, ErrorCodes.DuplicateFaculty)
    {
    }
}