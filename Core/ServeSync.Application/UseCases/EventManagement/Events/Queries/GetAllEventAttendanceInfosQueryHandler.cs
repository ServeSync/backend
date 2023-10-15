using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventAttendanceInfosQueryHandler : IQueryHandler<GetAllEventAttendanceInfosQuery, IEnumerable<EventAttendanceInfoDto>>
{
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly ISqlQuery _sqlQuery;
    
    public GetAllEventAttendanceInfosQueryHandler(
        IBasicReadOnlyRepository<Event, Guid> eventRepository,
        ISqlQuery sqlQuery)
    {
        _eventRepository = eventRepository;
        _sqlQuery = sqlQuery;
    }
    
    public async Task<IEnumerable<EventAttendanceInfoDto>> Handle(GetAllEventAttendanceInfosQuery request, CancellationToken cancellationToken)
    {
        if (!await _eventRepository.IsExistingAsync(request.EventId))
        {
            throw new EventNotFoundException(request.EventId);
        }
        
        return await _sqlQuery.QueryListAsync<EventAttendanceInfoDto>(
            "SELECT Id, Code, StartAt, EndAt, QrCodeUrl FROM EventAttendanceInfo WHERE EventId = @EventId", 
            new { EventId = request.EventId });
    }
}