using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class SyncEventsCommandHandler : ICommandHandler<SyncEventsCommand>
{
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly IBackGroundJobManager _backGroundJobManager;
    
    public SyncEventsCommandHandler(
        IBasicReadOnlyRepository<Event, Guid> eventRepository,
        IBackGroundJobManager backGroundJobManager)
    {
        _eventRepository = eventRepository;
        _backGroundJobManager = backGroundJobManager;
    }
    
    public async Task Handle(SyncEventsCommand request, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.FindAllAsync();
        foreach (var @event in events)
        {
            SyncEventData(@event.Id);
        }
    }
    
    private void SyncEventData(Guid id)
    {
        var job = new SyncEventReadModelBackGroundJob(id);
        _backGroundJobManager.Fire(job);
    }
}