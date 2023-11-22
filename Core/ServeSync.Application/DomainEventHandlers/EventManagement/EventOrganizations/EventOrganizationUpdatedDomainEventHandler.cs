using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class EventOrganizationUpdatedDomainEventHandler : IDomainEventHandler<EventOrganizationUpdatedDomainEvent>
{
    private readonly ITenantService _tenantService;
    private readonly ILogger<EventOrganizationUpdatedDomainEvent> _logger;
    
    public EventOrganizationUpdatedDomainEventHandler(
        ITenantService tenantService,
        ILogger<EventOrganizationUpdatedDomainEvent> logger)
    {
        _tenantService = tenantService;
        _logger = logger;
    }
    
    public async Task Handle(EventOrganizationUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _tenantService.UpdateUserInTenantAsync(
            notification.EventOrganization.IdentityId!,
            notification.EventOrganization.TenantId.GetValueOrDefault(), 
            notification.EventOrganization.Name,
            notification.EventOrganization.ImageUrl);
        
        await _tenantService.UpdateAsync(
            notification.EventOrganization.TenantId.GetValueOrDefault(), 
            notification.EventOrganization.Name, 
            notification.EventOrganization.ImageUrl);
        
        _logger.LogInformation("Updated tenant {TenantId} with name {Name} and image {ImageUrl}", notification.EventOrganization.TenantId, notification.EventOrganization.Name, notification.EventOrganization.ImageUrl);
    }
    
    
}