using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class OrganizationInvitationRepository : EfCoreRepository<OrganizationInvitation>, IOrganizationInvitationRepository
{
    public OrganizationInvitationRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<OrganizationInvitation?> FindByCodeAsync(string code)
    {
        return GetQueryable().FirstOrDefaultAsync(x => x.Code == code);
    }
}