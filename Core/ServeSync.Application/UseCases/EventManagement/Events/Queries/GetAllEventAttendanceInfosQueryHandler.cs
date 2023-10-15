using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventAttendanceInfosQueryHandler : IQueryHandler<GetAllEventAttendanceInfosQuery, IEnumerable<EventAttendanceInfoDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly ISqlQuery _sqlQuery;
    
    public GetAllEventAttendanceInfosQueryHandler(
        ICurrentUser currentUser,
        IBasicReadOnlyRepository<Event, Guid> eventRepository,
        ISqlQuery sqlQuery)
    {
        _currentUser = currentUser;
        _eventRepository = eventRepository;
        _sqlQuery = sqlQuery;
    }
    
    public async Task<IEnumerable<EventAttendanceInfoDto>> Handle(GetAllEventAttendanceInfosQuery request, CancellationToken cancellationToken)
    {
        if (!await _eventRepository.IsExistingAsync(request.EventId))
        {
            throw new EventNotFoundException(request.EventId);
        }
        
        var canViewAttendanceCode = await _currentUser.IsStudentAffairAsync() || await _currentUser.IsAdminAsync();
        
        return await _sqlQuery.QueryListAsync<EventAttendanceInfoDto>(
            await GetQueryStringAsync(), 
            new { EventId = request.EventId });
    }

    private async Task<string> GetQueryStringAsync()
    {
        var canViewAttendanceCode = await _currentUser.IsStudentAffairAsync() || await _currentUser.IsAdminAsync();
        return canViewAttendanceCode
            ? "SELECT Id, StartAt, EndAt, Code, QrCodeUrl FROM EventAttendanceInfo WHERE EventId = @EventId"
            : "SELECT Id, StartAt, EndAt FROM EventAttendanceInfo WHERE EventId = @EventId";
    }
}