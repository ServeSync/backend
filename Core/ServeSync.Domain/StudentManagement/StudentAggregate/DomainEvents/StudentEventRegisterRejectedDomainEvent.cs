using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

public class StudentEventRegisterRejectedDomainEvent : IDomainEvent
{
    public Guid StudentId { get; set; } 
    public Guid EventRoleId { get; set; }
    public string RejectReason { get; set; }

    public StudentEventRegisterRejectedDomainEvent(Guid studentId, Guid eventRoleId, string rejectReason)
    {
        StudentId = studentId;
        EventRoleId = eventRoleId;
        RejectReason = rejectReason;
    }
}