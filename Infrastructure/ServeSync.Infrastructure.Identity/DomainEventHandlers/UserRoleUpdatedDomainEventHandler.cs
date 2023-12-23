using Microsoft.Extensions.Logging;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Infrastructure.Identity.Caching.Interfaces;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.DomainEvents;

namespace ServeSync.Infrastructure.Identity.DomainEventHandlers;

public class UserRoleUpdatedDomainEventHandler : IDomainEventHandler<UserRoleUpdatedDomainEvent>
{
    private readonly IUserCacheManager _userCacheManager;
    private readonly ILogger<UserRoleUpdatedDomainEventHandler> _logger;
    public UserRoleUpdatedDomainEventHandler(IUserCacheManager userCacheManager,
        ILogger<UserRoleUpdatedDomainEventHandler> logger)
    {
        _userCacheManager = userCacheManager;
        _logger = logger;
    }
    
    public async Task Handle(UserRoleUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _userCacheManager.RemoveRolesAsync(notification.UserId, notification.TenantId);
        _logger.LogInformation("Removed roles for user {UserId} in tenant {TenantId}", notification.UserId, notification.TenantId);
    }
}