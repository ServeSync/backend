using System.Security.Claims;
using MediatR;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Queries;

namespace ServeSync.API.Authorization;

public class CurrentUser : ICurrentUser
{
    public string Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.UserId);
    public string Name => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.UserName);
    public string Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.Email);
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;

    public CurrentUser(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    public async Task<bool> IsInRoleAsync(string role)
    {
        var roles = await _mediator.Send(new GetAllRoleForUserQuery(Id));
        return roles.Contains(role);
    }

    public Task<bool> IsStudentAsync()
    {
        return IsInRoleAsync(AppRole.Student);
    }

    public Task<bool> IsAdminAsync()
    {
        return IsInRoleAsync(AppRole.Admin);
    }

    public string GetClaim(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
    }
}