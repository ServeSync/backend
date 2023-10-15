using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

public class StudentEventAttendance : Entity
{
    public Guid StudentEventRegisterId { get; private set; }
    public StudentEventRegister? StudentEventRegister { get; private set; }
    
    public Guid EventAttendanceInfoId { get; private set; }
    public EventAttendanceInfo? EventAttendanceInfo { get; private set; }
    
    public DateTime AttendanceAt { get; private set; }
    
    public StudentEventAttendance(Guid studentEventRegisterId, Guid eventAttendanceInfoId)
    {
        StudentEventRegisterId = Guard.NotNull(studentEventRegisterId, nameof(StudentEventRegisterId));
        EventAttendanceInfoId = Guard.NotNull(eventAttendanceInfoId, nameof(EventAttendanceInfoId));
        AttendanceAt = DateTime.Now;
    }
    
    private StudentEventAttendance()
    {
        
    }
}