using Microsoft.Extensions.Logging;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Jobs;

public class SyncEventOrganizationReadModelBackGroundJobHandler : IBackGroundJobHandler<SyncEventOrganizationReadModelBackGroundJob>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly ILogger<SyncEventOrganizationReadModelBackGroundJobHandler> _logger;
    
    public SyncEventOrganizationReadModelBackGroundJobHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IEventReadModelRepository eventReadModelRepository,
        ILogger<SyncEventOrganizationReadModelBackGroundJobHandler> logger)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventReadModelRepository = eventReadModelRepository;
        _logger = logger;
    }
    
    public async Task Handle(SyncEventOrganizationReadModelBackGroundJob notification, CancellationToken cancellationToken)
    {
        var eventOrganization = await _eventOrganizationRepository.FindByIdAsync(notification.EventOrganizationId);
        if (eventOrganization == null)
        {
            _logger.LogError("Event organization with id {EventOrganizationId} not found", notification.EventOrganizationId);
            throw new EventOrganizationNotFoundException(notification.EventOrganizationId);
        }
        
        await _eventReadModelRepository.UpdateOrganizationInEventsAsync(eventOrganization);
        _logger.LogInformation("Sync event organization read model success: event organization '{Id}'", notification.EventOrganizationId);
    }
}