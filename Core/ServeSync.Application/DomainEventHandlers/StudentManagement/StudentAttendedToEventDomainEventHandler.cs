using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class StudentAttendedToEventDomainEventHandler : IDomainEventHandler<StudentAttendedToEventDomainEvent>
{
    public StudentAttendedToEventDomainEventHandler(
       )
    {
    }
    
    public async Task Handle(StudentAttendedToEventDomainEvent notification, CancellationToken cancellationToken)
    {
        // Send email
    }
}