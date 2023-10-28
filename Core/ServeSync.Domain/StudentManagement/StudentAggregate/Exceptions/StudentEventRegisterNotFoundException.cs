using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class StudentEventRegisterNotFoundException : ResourceNotFoundException
{
    public StudentEventRegisterNotFoundException(Guid id) 
        : base($"Student registration with Id '{id}' does not exist!", ErrorCodes.StudentEventRegisterNotFound)
    {
    }
}