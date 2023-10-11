using ServeSync.Domain.EventManagement.EventCategoryAggregate;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class EventCategoryRepository : EfCoreRepository<EventCategory>, IEventCategoryRepository
{
    public EventCategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
        AddInclude(x => x.Activities);
    }
}