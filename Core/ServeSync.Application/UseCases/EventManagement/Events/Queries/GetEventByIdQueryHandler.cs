using ServeSync.Application.QueryObjects;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, EventDetailDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IEventQueries _eventQuery;
    
    public GetEventByIdQueryHandler(ICurrentUser currentUser, IEventQueries eventQuery)
    {
        _currentUser = currentUser;
        _eventQuery = eventQuery;
    }
    
    public async Task<EventDetailDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventQuery.GetEventDetailByIdAsync(request.EventId);
        if (@event == null)
        {
            throw new EventNotFoundException(request.EventId);
        }
        
        var canViewQrCode = await _currentUser.IsAdminAsync() || await _currentUser.IsStudentAffairAsync();
        if (!canViewQrCode)
        {
            @event.AttendanceInfos.ForEach(x =>
            {
                x.Code = null;
                x.QrCodeUrl = null;
            });
        }
        
        return @event;
    }
}