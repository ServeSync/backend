using System.Collections;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

public class UserInfoDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string AvatarUrl { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = null!;
    public IEnumerable<string> Permissions { get; set; } = null!;
}