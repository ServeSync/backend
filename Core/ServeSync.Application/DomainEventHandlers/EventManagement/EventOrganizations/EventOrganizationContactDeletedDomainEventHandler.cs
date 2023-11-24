using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class EventOrganizationContactDeletedDomainEventHandler : IDomainEventHandler<EventOrganizationContactDeletedDomainEvent>
{
    private readonly ITenantService _tenantService;
    private readonly ILogger<EventOrganizationDeletedDomainEventHandler> _logger;

    public EventOrganizationContactDeletedDomainEventHandler(
        ITenantService tenantService,
        ILogger<EventOrganizationDeletedDomainEventHandler> logger)
    {
        _tenantService = tenantService;
        _logger = logger;
    }

    public async Task Handle(EventOrganizationContactDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Status == OrganizationStatus.Active)
        {
            await _tenantService.RemoveUserFromTenantAsync(notification.IdentityId, notification.TenantId);    
            _logger.LogInformation("Removed user '{IdentityId} from tenant {tenantId}'!", notification.IdentityId, notification.TenantId);
        }
    }
}
