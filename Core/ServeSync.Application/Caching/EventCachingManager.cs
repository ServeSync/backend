using Microsoft.Extensions.Logging;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.Caching;

public class EventCachingManager : CachingManager<Event>, IEventCachingManager
{
    private readonly ICachingService _cacheService;
    private readonly IEventRepository _eventRepository;
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventReadOnlyRepository;
    private readonly ILogger<EventCachingManager> _logger;

    private const string EventOwnerByEvent = "Event.Owner";
    private const string EventOwnerByRegistration = "Event.Owner.Registration";
    private const string EventOwnerByEventRole = "Event.Owner.EventRole";
    
    public EventCachingManager(
        ICachingService cacheService,
        IEventRepository eventRepository,
        IBasicReadOnlyRepository<Event, Guid> eventReadOnlyRepository,
        ILogger<EventCachingManager> logger)
    {
        _cacheService = cacheService;
        _eventRepository = eventRepository;
        _eventReadOnlyRepository = eventReadOnlyRepository;
        _logger = logger;
    }
    
    public async Task<string?> GetOrAddEventOwnerAsync(Guid eventId)
    {
        var ownerId = await _cacheService.GetRecordAsync<string>($"{EventOwnerByEvent}:{eventId}");
        if (string.IsNullOrWhiteSpace(ownerId))
        {
            _logger.LogInformation("Cache missed event owner!");
            var @event = await _eventReadOnlyRepository.FindByIdAsync(eventId);
            if (@event == null)
            {
                _logger.LogWarning($"Event {eventId} not found!");
                return null;
            }
            
            ownerId = @event.CreatedBy;
            await _cacheService.SetRecordAsync($"{EventOwnerByEvent}:{eventId}", ownerId, TimeSpan.FromDays(DefaultCacheDurationInDay));
        }
        
        return ownerId;
    }

    public async Task<string?> GetOrAddEventOwnerByRegistrationAsync(Guid eventRegisterId)
    {
        var ownerId = await _cacheService.GetRecordAsync<string>($"{EventOwnerByRegistration}:{eventRegisterId}");
        if (string.IsNullOrWhiteSpace(ownerId))
        {
            _logger.LogInformation("Cache missed event owner!");
            var owner = await _eventRepository.GetEventOwnerByRegistrationAsync(eventRegisterId);
            if (string.IsNullOrEmpty(owner))
            {
                _logger.LogWarning($"Event with registration {eventRegisterId} not found!");
                return null;
            }
            
            ownerId = owner;
            await _cacheService.SetRecordAsync($"{EventOwnerByRegistration}:{eventRegisterId}", ownerId, TimeSpan.FromDays(DefaultCacheDurationInDay));
        }
        
        return ownerId;
    }

    public async Task<string?> GetOrAddEventOwnerByEventRoleAsync(Guid eventRoleId)
    {
        var ownerId = await _cacheService.GetRecordAsync<string>($"{EventOwnerByEventRole}:{eventRoleId}");
        if (string.IsNullOrWhiteSpace(ownerId))
        {
            _logger.LogInformation("Cache missed event owner!");
            var @event = await _eventReadOnlyRepository.FindAsync(new EventByRoleSpecification(eventRoleId));
            if (@event == null)
            {
                _logger.LogWarning($"Event with event role {eventRoleId} not found!");
                return null;
            }
            
            ownerId = @event.CreatedBy;
            await _cacheService.SetRecordAsync($"{EventOwnerByEventRole}:{eventRoleId}", ownerId, TimeSpan.FromDays(DefaultCacheDurationInDay));
        }
        
        return ownerId;
    }
}