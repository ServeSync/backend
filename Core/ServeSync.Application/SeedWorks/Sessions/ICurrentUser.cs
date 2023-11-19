namespace ServeSync.Application.SeedWorks.Sessions;

public interface ICurrentUser
{
    public string Id { get; }
    public string ReferenceId { get; }
    public string Name { get; }
    public string Email { get; }
    public bool IsAuthenticated { get; }

    Task<bool> IsInRoleAsync(string role);
    Task<bool> IsStudentAsync();
    Task<bool> IsAdminAsync();
    Task<bool> IsStudentAffairAsync();
    Task<bool> IsOrganizationAsync();
    Task<bool> IsOrganizationContactAsync();
    
    string? GetClaim(string claimType);
}