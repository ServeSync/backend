using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

public class UserInfoDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string AvatarUrl { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = null!;
}