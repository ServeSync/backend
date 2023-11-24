using System.Security.Claims;
using MediatR;
using ServeSync.Application.Common;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Queries;

namespace ServeSync.API.Authorization;

public class CurrentUser : ICurrentUser
{
    public string Id 
        => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.UserId);
    
    public string ReferenceId 
        => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.ReferenceId);
    
    public string Name 
        => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.UserName);
    
    public string Email 
        => _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.Email);
    
    public Guid TenantId
        => Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaim.TenantId), out var tenantId) ? tenantId : Guid.Empty;
    
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
        if (IsAuthenticated)
        {
            var roles = await _mediator.Send(new GetAllRoleForUserQuery(Id, TenantId));
            return roles.Contains(role);    
        }

        return false;
    }

    public Task<bool> IsStudentAsync()
    {
        return IsInRoleAsync(AppRole.Student);
    }
    
    public Task<bool> IsStudentAffairAsync()
    {
        return IsInRoleAsync(AppRole.StudentAffair);
    }

    public Task<bool> IsOrganizationAsync()
    {
        return IsInRoleAsync(AppRole.EventOrganization);
    }

    public Task<bool> IsOrganizationContactAsync()
    {
        return IsInRoleAsync(AppRole.EventOrganizer);
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