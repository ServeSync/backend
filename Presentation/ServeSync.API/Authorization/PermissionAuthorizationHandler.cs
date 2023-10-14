using Microsoft.AspNetCore.Authorization;
using ServeSync.Application.Identity;
using ServeSync.Application.SeedWorks.Sessions;

namespace ServeSync.API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(
        IIdentityService identityService,
        ICurrentUser currentUser,
        ILogger<PermissionAuthorizationHandler> logger)
    {
        _identityService = identityService;
        _currentUser = currentUser;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
    {
        if (!_currentUser.IsAuthenticated)
        {
            _logger.LogError("Authorized failed, user is not authenticated!");
            return;
        }

        var hasPermission = await _identityService.HasPermissionAsync(_currentUser.Id, requirement.Permission);
        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogError("Authorized failed, user does not has permission '{Permission}'!", requirement.Permission);
        }
    }
}