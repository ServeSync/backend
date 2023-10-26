using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class StudentNotInAttendEventAreaException : ResourceInvalidOperationException
{
    public StudentNotInAttendEventAreaException(Guid studentId, Guid eventId) 
        : base($"Student with id '{studentId}' is not in attend event area of event with id '{eventId}'", ErrorCodes.StudentNotInAttendEventArea)
    {
    }
}