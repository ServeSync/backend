using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServeSync.Infrastructure.EfCore.Repositories.Base;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class UserRepository : EfCoreRepository<ApplicationUser, string>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        AddInclude(x => x.RefreshToken);
        AddInclude(x => x.Tenants);
        AddInclude(x => x.Roles);
    }

    public Task<ApplicationUser?> FindByUserNameOrEmailAsync(string username, string email)
    {
        return DbSet.FirstOrDefaultAsync(x => x.UserName == username || x.Email == email);
    }

    public Task<ApplicationUser?> FindByUserNameOrEmailAndRoles(string username, string email, IEnumerable<string> roles)
    {
        var rolesQueryable = DbContext.Set<ApplicationRole>().Where(x => roles.Any(r => r == x.Name));
        var userRolesQueryable = DbContext.Set<ApplicationUserInRole>().Where(x => rolesQueryable.Any(r => r.Id == x.RoleId));

        return GetQueryable().FirstOrDefaultAsync(x => (x.UserName == username || x.Email == email) && userRolesQueryable.Any(r => r.UserId == x.Id));
    }

    public async Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken)
    {
        return (await DbContext.Set<RefreshToken>()
                               .Include(x => x.User)
                                    .ThenInclude(x => x.Tenants)
                               .Include(x => x.User)
                                    .ThenInclude(x => x.Roles)
                               .Include(x => x.User)
                                    .ThenInclude(x => x.RefreshToken)
                               .FirstOrDefaultAsync(x => x.Value == refreshToken))?.User;
    }

    public async Task<IList<string>> GetRolesAsync(string id, Guid tenantId)
    {
        return await DbContext.Database.SqlQueryRaw<string>(
                @"
                    SELECT Name From AspNetRoles
                    WHERE Id IN (SELECT RoleId FROM AspNetUserRoles WHERE TenantId = {0} AND UserId = {1})
                ", tenantId.ToString(), id)
            .ToListAsync();
    }
}