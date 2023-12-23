namespace ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

public class UserBasicInfoDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}