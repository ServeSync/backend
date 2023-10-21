using ServeSync.Application.ReadModels.Abstracts;

namespace ServeSync.Application.ReadModels.Events;

public interface IEventReadModelRepository : IReadModelRepository<EventReadModel, Guid>
{
    Task<EventRoleReadModel?> GetEventRoleByIdAsync(Guid eventRoleId);
    
    Task<List<EventRoleReadModel>?> GetEventRolesAsync(Guid eventId);
}