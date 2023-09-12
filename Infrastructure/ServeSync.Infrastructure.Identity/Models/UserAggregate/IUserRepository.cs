using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate;

public interface IUserRepository : IRepository<ApplicationUser, string>
{
    Task<ApplicationUser> FindByUserNameOrEmailAsync(string username, string email);
}