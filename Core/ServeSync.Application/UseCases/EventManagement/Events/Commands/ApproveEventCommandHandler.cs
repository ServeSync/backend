using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class ApproveEventCommandHandler : ICommandHandler<ApproveEventCommand, Guid>
{
    private readonly IEventRepository _eventRepository;
    private readonly IEventDomainService _eventDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveEventCommandHandler> _logger;

    public ApproveEventCommandHandler(
        IEventRepository eventRepository,
        IEventDomainService eventDomainService,
        IUnitOfWork unitOfWork,
        ILogger<ApproveEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _eventDomainService = eventDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> Handle(ApproveEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.FindByIdAsync(request.EventId);
        if (@event == null)
        {
            throw new EventNotFoundException(request.EventId);
        }
        
        _eventDomainService.ApproveEvent(@event, DateTime.UtcNow);
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Event with id {EventID} was approved!", request.EventId);

        return @event.Id;
    }
}