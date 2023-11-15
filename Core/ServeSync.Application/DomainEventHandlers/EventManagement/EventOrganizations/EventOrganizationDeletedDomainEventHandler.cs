using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class EventOrganizationDeletedDomainEventHandler : IDomainEventHandler<EventOrganizationDeletedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<EventOrganizationDeletedDomainEventHandler> _logger;
    
    public EventOrganizationDeletedDomainEventHandler(
        IIdentityService identityService,
        ILogger<EventOrganizationDeletedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }
    
    public async Task Handle(EventOrganizationDeletedDomainEvent @event, CancellationToken cancellationToken)
    {
        var result = await _identityService.DeleteAsync(@event.IdentityId);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Identity with id {IdentityId} was deleted!", @event.IdentityId);
        }
        else
        {
            _logger.LogError("Identity with id {IdentityId} was not deleted: {Message}", @event.IdentityId, result.Error);
        }
    }
}