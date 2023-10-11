using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class EventCollaborationRequestRepository : EfCoreRepository<EventCollaborationRequest>, IEventCollaborationRequestRepository
{
    public EventCollaborationRequestRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}