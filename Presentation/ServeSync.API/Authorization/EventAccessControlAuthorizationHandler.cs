using Microsoft.AspNetCore.Authorization;
using ServeSync.API.Common.Enums;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.Common;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Authorization;

public class EventAccessControlAuthorizationHandler : AuthorizationHandler<EventAccessControlRequirement>
{
    private readonly ICurrentUser _currentUser;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventCachingManager _eventCachingManager;
    private readonly ILogger<RoleAuthorizationHandler> _logger;

    public EventAccessControlAuthorizationHandler(
        ICurrentUser currentUser, 
        IHttpContextAccessor httpContextAccessor,
        ILogger<RoleAuthorizationHandler> logger,
        IEventCachingManager eventCachingManager)
    {
        _currentUser = currentUser;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _eventCachingManager = eventCachingManager;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EventAccessControlRequirement requirement)
    {
        if (!_currentUser.IsAuthenticated)
        {
            _logger.LogError("Authorized failed, user is not authenticated!");
            return;
        }

        var isValidRole = await _currentUser.IsAdminAsync() || await _currentUser.IsStudentAffairAsync();
        if (isValidRole)
        {
            context.Succeed(requirement);
            return;
        }
        else
        {
            _logger.LogError("User is not in role '{Role1}', '{Role2}'!", AppRole.Admin, AppRole.StudentAffair);
        }
        
        var eventOwnerId = await GetEventOwnerFromSourceAsync(requirement.Source, context);
        if (eventOwnerId == null)
        {
            _logger.LogError("Authorize failed, Event by {Source} does not exist!", requirement.Source);
        }
        else
        {
            if (eventOwnerId == _currentUser.Id)
            {
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogError("Authorized failed, user is not the owner of the event!");
            }
        }
    }

    private async Task<string?> GetEventOwnerFromSourceAsync(EventSourceAccessControl source, AuthorizationHandlerContext context)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        if (source == EventSourceAccessControl.Event)
        {
            var eventId = httpContext.Request.RouteValues["id"] as string ?? httpContext.Request.RouteValues["eventId"] as string;
            if (string.IsNullOrEmpty(eventId))
            {
                _logger.LogError("Event Id is null or empty!");
                return null;
            }
            
            return (await _eventCachingManager.GetOrAddEventOwnerAsync(Guid.Parse(eventId)));
        }
        else if (source == EventSourceAccessControl.EventRole)
        {
            var eventRoleId = httpContext.Request.RouteValues["id"] as string ?? httpContext.Request.RouteValues["eventRoleId"] as string;
            if (string.IsNullOrEmpty(eventRoleId))
            {
                _logger.LogError("Event Role Id is null or empty!");
                return null;
            }

            return (await _eventCachingManager.GetOrAddEventOwnerByEventRoleAsync(Guid.Parse(eventRoleId)));
        }
        else if (source == EventSourceAccessControl.EventRegister)
        {
            var eventRegisterId = httpContext.Request.RouteValues["id"] as string ?? httpContext.Request.RouteValues["eventRegisterId"] as string;
            if (string.IsNullOrEmpty(eventRegisterId))
            {
                _logger.LogError("Event Register Id is null or empty!");
                return null;
            }

            return (await _eventCachingManager.GetOrAddEventOwnerByRegistrationAsync(Guid.Parse(eventRegisterId)));
        }
        return null;
    }
}