using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate;

public interface IUserRepository : IRepository<ApplicationUser, string>
{
    Task<ApplicationUser?> FindByUserNameOrEmailAsync(string username, string email);

    Task<ApplicationUser?> FindByUserNameOrEmailAndRoles(string username, string email, IEnumerable<string> roles);
    
    Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken);
}