using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Domain.EventManagement.EventAggregate;

public interface IEventRepository : IRepository<Event>
{
    
}