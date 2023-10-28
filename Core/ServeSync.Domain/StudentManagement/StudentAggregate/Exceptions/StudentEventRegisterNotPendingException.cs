using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class StudentEventRegisterNotPendingException : ResourceInvalidOperationException
{
    public StudentEventRegisterNotPendingException(Guid id) 
        : base($"Registration with id '{id}' not in pending status!", ErrorCodes.StudentEventRegisterNotPending)
    {
    }
}