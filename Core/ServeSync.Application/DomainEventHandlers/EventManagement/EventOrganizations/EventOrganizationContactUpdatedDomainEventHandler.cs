using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class EventOrganizationContactUpdatedDomainEventHandler : IDomainEventHandler<EventOrganizationContactUpdatedDomainEvent>
{
    private readonly ITenantService _tenantService;
    private readonly ILogger<EventOrganizationContactUpdatedDomainEventHandler> _logger;
    
    public EventOrganizationContactUpdatedDomainEventHandler(
        ITenantService tenantService,
        ILogger<EventOrganizationContactUpdatedDomainEventHandler> logger)
    {
        _tenantService = tenantService;
        _logger = logger;
    }
    
    public async Task Handle(EventOrganizationContactUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(notification.EventOrganizationContact.IdentityId))
        {
            await _tenantService.UpdateUserInTenantAsync(
                notification.EventOrganizationContact.IdentityId!,
                notification.EventOrganizationContact.EventOrganization!.TenantId.GetValueOrDefault(), 
                notification.EventOrganizationContact.Name,
                notification.EventOrganizationContact.ImageUrl);
            
            _logger.LogInformation("Updated user {IdentityId} in tenant {TenantId} with name {Name} and image {ImageUrl}", notification.EventOrganizationContact.IdentityId, notification.EventOrganizationContact.EventOrganization.TenantId, notification.EventOrganizationContact.Name, notification.EventOrganizationContact.ImageUrl);
        }
    }
}