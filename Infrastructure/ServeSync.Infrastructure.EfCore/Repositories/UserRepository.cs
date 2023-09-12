using Microsoft.EntityFrameworkCore;
using ServeSync.Infrastructure.EfCore.Repositories.Base;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class UserRepository : EfCoreRepository<ApplicationUser, string>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<ApplicationUser> FindByUserNameOrEmailAsync(string username, string email)
    {
        return DbSet.FirstOrDefaultAsync(x => x.UserName == username || x.Email == email);
    }
}