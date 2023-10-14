using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class StudentHasAlreadyApprovedToEventException : ResourceAlreadyExistException
{
    public StudentHasAlreadyApprovedToEventException(Guid id, Guid eventId) 
        : base($"Student wih Id '{id}' has already been approved to Event {eventId}", ErrorCodes.StudentHasAlreadyApprovedToEvent)
    {
    }
}