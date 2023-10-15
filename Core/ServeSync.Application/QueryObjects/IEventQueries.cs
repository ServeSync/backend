using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.QueryObjects;

public interface IEventQueries
{
    Task<EventDetailDto?> GetEventDetailByIdAsync(Guid eventId);
}