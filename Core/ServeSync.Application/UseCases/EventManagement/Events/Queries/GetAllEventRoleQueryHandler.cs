using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventRoleQueryHandler : IQueryHandler<GetAllEventRolesQuery, IEnumerable<EventRoleDto>>
{
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly ISqlQuery _sqlQuery;
    
    public GetAllEventRoleQueryHandler(
        IBasicReadOnlyRepository<Event, Guid> eventRepository,
        ISqlQuery sqlQuery)
    {
        _eventRepository = eventRepository;
        _sqlQuery = sqlQuery;
    }
    
    public async Task<IEnumerable<EventRoleDto>> Handle(GetAllEventRolesQuery request, CancellationToken cancellationToken)
    {
        if (!await _eventRepository.IsExistingAsync(request.EventId))
        {
            throw new EventNotFoundException(request.EventId);
        }
        
        return await _sqlQuery.QueryListAsync<EventRoleDto>(
            GetQueryString(), 
            new { EventId = request.EventId, RegisterStatus = EventRegisterStatus.Approved });
    }

    private string GetQueryString()
    {
        return @"
            SELECT EventRole.Id, EventRole.Name, EventRole.Description, EventRole.IsNeedApprove, EventRole.Score, EventRole.Quantity, Count(EventRole.Id) as Registered From EventRole
            INNER JOIN StudentEventRegister
            On EventRole.Id = StudentEventRegister.EventRoleId
            WHERE EventRole.EventId = @EventId and StudentEventRegister.Status = @RegisterStatus
            Group by EventRole.Id
        ";
    }
}