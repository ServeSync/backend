using Microsoft.AspNetCore.Authorization;
using ServeSync.Application.Identity;
using ServeSync.Application.SeedWorks.Sessions;

namespace ServeSync.API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUser _currentUser;

    public PermissionAuthorizationHandler(
        IIdentityService identityService,
        ICurrentUser currentUser)
    {
        _identityService = identityService;
        _currentUser = currentUser;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
    {
        if (!_currentUser.IsAuthenticated)
        {
            return;
        }

        var permissions = await _identityService.GetPermissionsForUserAsync(_currentUser.Id);

        var isAuthorized = permissions.Contains(requirement.Permission);
        if (isAuthorized)
        {
            context.Succeed(requirement);
        }   
    }
}