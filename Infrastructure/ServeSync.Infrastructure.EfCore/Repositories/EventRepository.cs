using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class EventRepository : EfCoreRepository<Event>, IEventRepository
{
    public EventRepository(AppDbContext dbContext) : base(dbContext)
    {
        AddInclude(x => x.Organizations);
        AddInclude(x => x.AttendanceInfos);
        AddInclude(x => x.Roles);
        AddInclude(x => x.RegistrationInfos);
        AddInclude("Organizations.Representatives");
    }

    public async Task<IList<Student>> GetRegisteredStudentAsync(Guid eventId)
    {
        return await GetRegisteredStudentQueryable(eventId).ToListAsync();
    }

    public async Task<string> GetEventOwnerByRegistrationAsync(Guid eventRegisterId)
    {
        return (await (DbContext.Set<StudentEventRegister>().Include(x => x.EventRole)
            .ThenInclude(x => x.Event)
            .FirstOrDefaultAsync(x => x.Id == eventRegisterId)))
            ?.CreatedBy;
    }

    public Task<bool> IsStudentRegisteredToEventAsync(Guid studentId, Guid eventId, Guid eventRoleId)
    {
        return GetRegisteredStudentQueryable(eventId, eventRoleId)
            .AnyAsync(x => x.Id == studentId);
    }

    public Task<bool> IsStudentAttendedToEventAsync(Guid studentId, Guid eventId)
    {
        var eventRoleQueryable = DbContext.Set<EventRole>()
            .Where(x => x.EventId == eventId);

        return DbContext.Set<StudentEventRegister>()
            .AnyAsync(x => x.StudentId == studentId && 
                        eventRoleQueryable.Any(y => y.Id == x.EventRoleId) &&
                        x.StudentEventAttendance != null);
    }

    private IQueryable<Student> GetRegisteredStudentQueryable(Guid eventId, Guid? eventRoleId = null)
    {
        var eventRoleQueryable = DbContext.Set<EventRole>()
            .Where(x => x.EventId == eventId && (eventRoleId == null || x.Id == eventRoleId));
        
        var studentQueryable = DbContext.Set<Student>()
            .Where(x => x.EventRegisters.Any(y => y.Status == EventRegisterStatus.Approved && eventRoleQueryable.Any(e => e.Id == y.EventRoleId)));
        return studentQueryable;
    }
}