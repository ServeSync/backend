using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Jobs;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class SyncEventOrganizationContactReadModelDomainEventHandler 
    : IPersistedDomainEventHandler<EventOrganizationContactUpdatedDomainEvent>
{
    private readonly IBackGroundJobManager _backGroundJobManager;
    
    public SyncEventOrganizationContactReadModelDomainEventHandler(IBackGroundJobManager backGroundJobManager)
    {
        _backGroundJobManager = backGroundJobManager;
    }
    
    public Task Handle(EventOrganizationContactUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        var job = new SyncEventOrganizationContactReadModelBackGroundJob(@event.EventOrganizationContact.Id);
        _backGroundJobManager.Fire(job);

        return Task.CompletedTask;
    }
}