using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventAttendanceInfosQuery : IQuery<IEnumerable<EventAttendanceInfoDto>>
{
    public Guid EventId { get; }

    public GetAllEventAttendanceInfosQuery(Guid eventId)
    {
        EventId = eventId;
    }
}