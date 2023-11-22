using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate;

public interface IOrganizationInvitationRepository : IRepository<OrganizationInvitation>
{
    Task<OrganizationInvitation> FindByCodeAsync(string code, InvitationType type);
}