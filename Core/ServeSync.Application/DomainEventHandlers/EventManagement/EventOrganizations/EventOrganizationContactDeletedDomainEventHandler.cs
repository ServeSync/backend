using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class EventOrganizationContactDeletedDomainEventHandler : IDomainEventHandler<EventOrganizationContactDeletedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<EventOrganizationDeletedDomainEventHandler> _logger;

    public EventOrganizationContactDeletedDomainEventHandler(
        IIdentityService identityService,
        ILogger<EventOrganizationDeletedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task Handle(EventOrganizationContactDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var result = await _identityService.DeleteAsync(notification.IdentityId);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Identity with id {IdentityId} was deleted!", notification.IdentityId);
        }
        else
        {
            _logger.LogError("Identity with id {IdentityId} was not deleted: {Message}", notification.IdentityId,
                result.Error);
        }
    }
}
