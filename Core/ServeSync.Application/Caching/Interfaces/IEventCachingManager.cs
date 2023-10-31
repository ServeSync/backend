namespace ServeSync.Application.Caching.Interfaces;

public interface IEventCachingManager
{
    Task<string?> GetOrAddEventOwnerAsync(Guid eventId);
    
    Task<string?> GetOrAddEventOwnerByRegistrationAsync(Guid eventRegisterId);
    
    Task<string?> GetOrAddEventOwnerByEventRoleAsync(Guid eventId);
}