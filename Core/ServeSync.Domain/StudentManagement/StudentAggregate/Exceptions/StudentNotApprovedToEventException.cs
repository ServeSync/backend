using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

public class StudentNotApprovedToEventException : ResourceInvalidDataException
{
    public StudentNotApprovedToEventException(Guid id, Guid eventId) 
        : base($"Student with Id '{id}' has not been approved or registered to event {eventId} yet!", ErrorCodes.StudentNotApprovedToEvent)
    {
    }
}