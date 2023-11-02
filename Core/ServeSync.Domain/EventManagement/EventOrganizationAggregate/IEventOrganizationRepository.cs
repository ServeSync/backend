using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate;

public interface IEventOrganizationRepository : IRepository<EventOrganization>
{
    Task<EventOrganization?> FindByEmailAsync(string email);
}