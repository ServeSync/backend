using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class RejectEventCommandHandler : ICommandHandler<RejectEventCommand>
{
    private readonly IEventRepository _eventRepository;
    private readonly IEventDomainService _eventDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectEventCommandHandler> _logger;
    
    public RejectEventCommandHandler(
        IEventRepository eventRepository, 
        IEventDomainService eventDomainService,
        IUnitOfWork unitOfWork, 
        ILogger<RejectEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _eventDomainService = eventDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(RejectEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.FindByIdAsync(request.EventId);
        if (@event == null)
        {
            throw new EventNotFoundException(request.EventId);
        }

        _eventDomainService.Reject(@event);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Event {EventId} has been rejected!", request.EventId);
    }
}