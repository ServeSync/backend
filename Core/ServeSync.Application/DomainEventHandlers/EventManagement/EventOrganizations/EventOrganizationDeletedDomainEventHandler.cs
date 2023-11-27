using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class EventOrganizationDeletedDomainEventHandler : IDomainEventHandler<EventOrganizationDeletedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly ITenantService _tenantService;
    private readonly ILogger<EventOrganizationDeletedDomainEventHandler> _logger;
    
    public EventOrganizationDeletedDomainEventHandler(
        IIdentityService identityService,
        ITenantService tenantService,
        ILogger<EventOrganizationDeletedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _tenantService = tenantService;
        _logger = logger;
    }
    
    public async Task Handle(EventOrganizationDeletedDomainEvent @event, CancellationToken cancellationToken)
    {
        if (@event.EventOrganization.Status == OrganizationStatus.Active
            && @event.EventOrganization.TenantId.HasValue
            && !string.IsNullOrEmpty(@event.EventOrganization.IdentityId))
        {
            await RemoveIdentityAsync(@event.EventOrganization.IdentityId, @event.EventOrganization.TenantId.Value);
            await RemoveTenantAsync(@event.EventOrganization.TenantId.Value);    
        }
    }
    
    private async Task RemoveTenantAsync(Guid tenantId)
    {
        await _tenantService.DeleteAsync(tenantId);
        _logger.LogInformation("Tenant with id {TenantId} was deleted!", tenantId);
    }

    private async Task RemoveIdentityAsync(string identityId, Guid tenantId)
    {
        await _tenantService.RemoveUserFromTenantAsync(identityId, tenantId);
        _logger.LogInformation("Removed user '{IdentityId} from tenant {tenantId}'!", identityId, tenantId);
    }
}