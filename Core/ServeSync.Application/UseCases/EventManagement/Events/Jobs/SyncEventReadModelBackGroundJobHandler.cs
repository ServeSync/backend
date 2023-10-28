using Microsoft.Extensions.Logging;
using Polly;
using ServeSync.Application.QueryObjects;
using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class SyncEventReadModelBackGroundJobHandler : IBackGroundJobHandler<SyncEventReadModelBackGroundJob>
{
    private readonly IEventQueries _eventQueries;
    private readonly IReadModelRepository<EventReadModel, Guid> _eventReadModelRepository;
    private readonly ILogger<SyncEventReadModelBackGroundJobHandler> _logger; 
    
    public SyncEventReadModelBackGroundJobHandler(
        IEventQueries eventQueries,
        IReadModelRepository<EventReadModel, Guid> eventReadModelRepository,
        ILogger<SyncEventReadModelBackGroundJobHandler> logger)
    {
        _eventQueries = eventQueries;
        _eventReadModelRepository = eventReadModelRepository;
        _logger = logger;
    }
    
    public async Task Handle(SyncEventReadModelBackGroundJob notification, CancellationToken cancellationToken)
    {
        if (notification.EventId == Guid.Empty)
        {
            return;
        }
        
        var policy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    _logger.LogError("Sync data for event '{EventId}' failed: {error}", notification.EventId, ex.Message);
                }
            );
        
        await policy.ExecuteAsync(async () =>
        {
            var @event = await _eventQueries.GetEventReadModelByIdAsync(notification.EventId);
            if (@event != null)
            {
                await _eventReadModelRepository.CreateOrUpdateAsync(@event);
                
                _logger.LogInformation("Sync data for event '{EventId}' success", notification.EventId);
            }
            else
            {
                throw new EventNotFoundException(notification.EventId);    
            }
        });
    }
}