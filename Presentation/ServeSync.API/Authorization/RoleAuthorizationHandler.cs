using Microsoft.AspNetCore.Authorization;
using ServeSync.Application.SeedWorks.Sessions;

namespace ServeSync.API.Authorization;

public class RoleAuthorizationHandler : AuthorizationHandler<RoleAuthorizationRequirement>
{
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<RoleAuthorizationHandler> _logger;

    public RoleAuthorizationHandler(ICurrentUser currentUser, ILogger<RoleAuthorizationHandler> logger)
    {
        _currentUser = currentUser;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAuthorizationRequirement requirement)
    {
        if (!_currentUser.IsAuthenticated)
        {
            _logger.LogError("Authorized failed, user is not authenticated!");
            return;
        }

        var hasRole = await _currentUser.IsInRoleAsync(requirement.Role);
        if (hasRole)
        {
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogError("Authorized failed, user is not in role '{Role}'!", requirement.Role);
        }
    }
}