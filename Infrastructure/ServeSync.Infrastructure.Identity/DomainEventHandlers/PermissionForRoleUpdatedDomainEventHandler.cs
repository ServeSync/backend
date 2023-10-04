using Microsoft.Extensions.Logging;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Infrastructure.Identity.Caching.Interfaces;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.DomainEvents;

namespace ServeSync.Infrastructure.Identity.DomainEventHandlers;

public class PermissionForRoleUpdatedDomainEventHandler : IDomainEventHandler<PermissionForRoleUpdatedDomainEvent>
{
    private readonly IPermissionCacheManager _permissionCacheManager;
    private readonly ILogger<PermissionForRoleUpdatedDomainEventHandler> _logger;
    
    public PermissionForRoleUpdatedDomainEventHandler(
        IPermissionCacheManager permissionCacheManager,
        ILogger<PermissionForRoleUpdatedDomainEventHandler> logger)
    {
        _permissionCacheManager = permissionCacheManager;
        _logger = logger;
    }
    
    public async Task Handle(PermissionForRoleUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Cache] Remove permission cache data for role {Role}!", notification.Name);
        
        await _permissionCacheManager.RemoveForRoleAsync(notification.Name);
    }
}