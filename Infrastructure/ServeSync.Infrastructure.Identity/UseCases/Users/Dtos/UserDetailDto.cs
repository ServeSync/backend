using ServeSync.Infrastructure.Identity.UseCases.Tenants.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

public class UserDetailDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IEnumerable<TenantDto> Tenants { get; set; } = null!;
}