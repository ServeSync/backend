using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class StudentAlreadyAttendanceException : ResourceAlreadyExistException
{
    public StudentAlreadyAttendanceException(Guid id) 
        : base($"Student has already attendance to event attendance {id}!", ErrorCodes.StudentAlreadyAttendance)
    {
    }
}