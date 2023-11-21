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
    }

    public Task<ApplicationUser?> FindByUserNameOrEmailAsync(string username, string email)
    {
        return DbSet.FirstOrDefaultAsync(x => x.UserName == username || x.Email == email);
    }

    public Task<ApplicationUser?> FindByUserNameOrEmailAndRoles(string username, string email, IEnumerable<string> roles)
    {
        var rolesQueryable = DbContext.Set<ApplicationRole>().Where(x => roles.Any(r => r == x.Name));
        var userRolesQueryable = DbContext.Set<IdentityUserRole<string>>().Where(x => rolesQueryable.Any(r => r.Id == x.RoleId));

        return GetQueryable().FirstOrDefaultAsync(x => (x.UserName == username || x.Email == email) && userRolesQueryable.Any(r => r.UserId == x.Id));
    }

    public async Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken)
    {
        return (await DbContext.Set<RefreshToken>()
                               .Include(x => x.User)
                               .FirstOrDefaultAsync(x => x.Value == refreshToken))?.User;
    }
}