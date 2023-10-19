using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.Events;

public class EventCancelledDomainEventHandler : IDomainEventHandler<EventCancelledDomainEvent>
{
    private readonly IEventRepository _eventRepository;
    private readonly IBackGroundJobManager _backGroundJobManager;

    public EventCancelledDomainEventHandler(
        IEventRepository eventRepository, 
        IBackGroundJobManager backGroundJobManager)
    {
        _eventRepository = eventRepository;
        _backGroundJobManager = backGroundJobManager;
    }
    
    public async Task Handle(EventCancelledDomainEvent @event, CancellationToken cancellationToken)
    {
        var registeredStudent = await _eventRepository.GetRegisteredStudentAsync(@event.Event.Id);
        var emails = registeredStudent.Select(s => s.Email).ToList();
        _backGroundJobManager.Fire(new SendMailCancelledEventToStudentBackGroundJob(@event.Event.Name, emails));
    }
}