using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

public record StudentAttendedToEventDomainEvent : EquatableDomainEvent
{
    public Student Student { get; set; }  
    public Guid EventRoleId { get; set; }

    public StudentAttendedToEventDomainEvent(Student student, Guid eventRoleId)
    {
        Student = student;
        EventRoleId = eventRoleId;
    }
}