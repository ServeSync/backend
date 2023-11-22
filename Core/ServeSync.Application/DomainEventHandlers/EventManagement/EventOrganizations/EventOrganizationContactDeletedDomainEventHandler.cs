using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class EventOrganizationContactDeletedDomainEventHandler : IDomainEventHandler<EventOrganizationContactDeletedDomainEvent>
{
    private readonly ITenantService _tenantService;
    private readonly ILogger<EventOrganizationDeletedDomainEventHandler> _logger;

    public EventOrganizationContactDeletedDomainEventHandler(
        IIdentityService identityService,
        ITenantService tenantService,
        ILogger<EventOrganizationDeletedDomainEventHandler> logger)
    {
        _tenantService = tenantService;
        _logger = logger;
    }

    public async Task Handle(EventOrganizationContactDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _tenantService.RemoveUserFromTenantAsync(notification.IdentityId, notification.TenantId);
    }
}
