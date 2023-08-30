namespace PBL6.Application.SeedWorks.Sessions;

public interface ICurrentUser
{
    public string Id { get; }
    public string Name { get; }
    public string Email { get; }
    public bool IsAuthenticated { get; }

    bool IsInRole(string role);
    string? GetClaim(string claimType);
}