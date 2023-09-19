using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

public class UserInfoDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<string> Roles { get; set; }
}