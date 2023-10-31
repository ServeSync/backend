using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.EventManagement.EventAggregate;

public interface IEventRepository : IRepository<Event>
{
    public Task<IList<Student>> GetRegisteredStudentAsync(Guid eventId);
    
    public Task<string> GetEventOwnerByRegistrationAsync(Guid eventRegisterId);
}