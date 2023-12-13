using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.EventManagement.EventAggregate;

public interface IEventRepository : IRepository<Event>
{
    Task<IList<Student>> GetRegisteredStudentAsync(Guid eventId);
    
    Task<string> GetEventOwnerByRegistrationAsync(Guid eventRegisterId);
    
    Task<bool> IsStudentRegisteredToEventAsync(Guid studentId, Guid eventId, Guid eventRoleId);
    
    Task<bool> IsStudentAttendedToEventAsync(Guid studentId, Guid eventId);
}