using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class StudentRegisteredToEventRoleException : ResourceInvalidOperationException
{
    public StudentRegisteredToEventRoleException(Guid id, Guid eventRoleId) 
        : base($"Student with Id '{id}' has already registered to event role with Id '{eventRoleId}'", ErrorCodes.StudentRegisteredToEventRole)
    {
    }
}