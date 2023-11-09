using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

public class StudentEventRegister : AuditableEntity
{
    public string? Description { get; private set; }
    public string? RejectReason { get; private set; }
    public EventRegisterStatus Status { get; private set; }
    
    public Guid EventRoleId { get; private set; }
    public EventRole? EventRole { get; private set; }
    
    public Guid StudentId { get; private set; }
    public Student? Student { get; private set; }
    
    public StudentEventAttendance? StudentEventAttendance { get; private set; }
    
    internal StudentEventRegister(
        string? description,
        Guid eventRoleId,
        Guid studentId)
    {
        Description = description;
        EventRoleId = Guard.NotNull(eventRoleId, nameof(EventRoleId));
        StudentId = Guard.NotNull(studentId, nameof(StudentId));
        Status = EventRegisterStatus.Pending;
    }
    
    internal void Approve()
    {
        if (Status != EventRegisterStatus.Pending)
        {
            throw new StudentEventRegisterNotPendingException(Id);
        }

        Status = EventRegisterStatus.Approved;
        AddDomainEvent(new StudentEventRegisterApprovedDomainEvent(StudentId, EventRoleId));
    }
    
    internal void Reject(string reason)
    {
        if (Status != EventRegisterStatus.Pending)
        {
            throw new StudentEventRegisterNotPendingException(Id);
        }
        
        Status = EventRegisterStatus.Rejected;
        RejectReason = reason;
    }
    
    internal void Attendance(Guid eventAttendanceInfoId)
    {
        if (StudentEventAttendance != null)
        {
            throw new StudentAlreadyAttendanceException(eventAttendanceInfoId);
        }
        
        StudentEventAttendance = new StudentEventAttendance(Id, eventAttendanceInfoId);
        AddDomainEvent(new StudentAttendedToEventDomainEvent(Student!, EventRoleId));
    }

    private StudentEventRegister()
    {
        
    }
}