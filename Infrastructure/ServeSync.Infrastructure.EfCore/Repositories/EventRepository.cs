using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class EventRepository : EfCoreRepository<Event>, IEventRepository
{
    public EventRepository(AppDbContext dbContext) : base(dbContext)
    {
        AddInclude(x => x.Organizations);
        AddInclude(x => x.AttendanceInfos);
    }
}