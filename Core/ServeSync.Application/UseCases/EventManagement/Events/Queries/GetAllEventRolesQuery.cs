using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventRolesQuery : IQuery<IEnumerable<EventRoleDto>>
{
    public Guid EventId { get; set; }
    
    public GetAllEventRolesQuery(Guid eventId)
    {
        EventId = eventId;
    }
}