using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

public class StudentEventRegisterApprovedDomainEvent : IDomainEvent
{
    public Guid StudentId { get; set; } 
    public Guid EventRoleId { get; set; }

    public StudentEventRegisterApprovedDomainEvent(Guid studentId, Guid eventRoleId)
    {
        StudentId = studentId;
        EventRoleId = eventRoleId;
    }
}