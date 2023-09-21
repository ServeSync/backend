using System.Security.Claims;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Authorization;

public class CurrentUser : ICurrentUser
{
    public string Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.UserId);
    public string Name => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.UserName);
    public string Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.Email);
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    }

    public string GetClaim(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
    }
}