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
        if (@event.Status == OrganizationStatus.Active)
        {
            await RemoveIdentityAsync(@event.IdentityId);
            await RemoveTenantAsync(@event.TenantId);    
        }
    }
    
    private async Task RemoveTenantAsync(Guid tenantId)
    {
        await _tenantService.DeleteAsync(tenantId);
        _logger.LogInformation("Tenant with id {TenantId} was deleted!", tenantId);
    }

    private async Task RemoveIdentityAsync(string identityId)
    {
        var result = await _identityService.DeleteAsync(identityId);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Identity with id {IdentityId} was deleted!", identityId);
        }
        else
        {
            _logger.LogError("Identity with id {IdentityId} was not deleted: {Message}", identityId, result.Error);
        }
    }
}