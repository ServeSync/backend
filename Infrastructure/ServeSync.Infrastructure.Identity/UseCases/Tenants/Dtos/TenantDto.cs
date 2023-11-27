namespace ServeSync.Infrastructure.Identity.UseCases.Tenants.Dtos;

public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? AvatarUrl { get; set; }
}