using Microsoft.Extensions.Logging;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Jobs;

public class SyncEventOrganizationContactReadModelBackGroundJobHandler 
    : IBackGroundJobHandler<SyncEventOrganizationContactReadModelBackGroundJob>
{
    private readonly IBasicReadOnlyRepository<EventOrganizationContact, Guid> _eventOrganizationContactRepository;
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly ILogger<SyncEventOrganizationContactReadModelBackGroundJobHandler> _logger;
    
    public SyncEventOrganizationContactReadModelBackGroundJobHandler(
        IBasicReadOnlyRepository<EventOrganizationContact, Guid> eventOrganizationContactRepository,
        IEventReadModelRepository eventReadModelRepository,
        ILogger<SyncEventOrganizationContactReadModelBackGroundJobHandler> logger)
    {
        _eventOrganizationContactRepository = eventOrganizationContactRepository;
        _eventReadModelRepository = eventReadModelRepository;
        _logger = logger;
    }
    
    public async Task Handle(SyncEventOrganizationContactReadModelBackGroundJob notification, CancellationToken cancellationToken)
    {
        var organizationContact =await _eventOrganizationContactRepository.FindByIdAsync(notification.EventOrganizationContactId);
        if (organizationContact == null)
        {
            _logger.LogError("Sync Event organization contact failed: id {EventOrganizationContactId} not found", notification.EventOrganizationContactId);
            throw new EventOrganizationContactNotFoundException(notification.EventOrganizationContactId);
        }
        await _eventReadModelRepository.UpdateOrganizationContactInEventsAsync(organizationContact);
        _logger.LogInformation("Sync event organization contact read model success: event organization contact '{Id}'", notification.EventOrganizationContactId);
    }
}