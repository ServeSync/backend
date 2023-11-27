using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Jobs;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class SyncEventOrganizationReadModelDomainEventHandler : IPersistedDomainEventHandler<EventOrganizationUpdatedDomainEvent>
{
    private readonly IBackGroundJobManager _backGroundJobManager;
    private readonly ILogger<SyncEventOrganizationReadModelDomainEventHandler> _logger;
    
    public SyncEventOrganizationReadModelDomainEventHandler(
        IBackGroundJobManager backGroundJobManager,
        ILogger<SyncEventOrganizationReadModelDomainEventHandler> logger)
    {
        _backGroundJobManager = backGroundJobManager;
        _logger = logger;
    }
    
    public Task Handle(EventOrganizationUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        var job = new SyncEventOrganizationReadModelBackGroundJob(@event.EventOrganization.Id);
        _backGroundJobManager.Fire(job);

        return Task.CompletedTask;
    }
}