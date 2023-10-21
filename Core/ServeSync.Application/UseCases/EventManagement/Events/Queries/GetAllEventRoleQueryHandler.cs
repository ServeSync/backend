using AutoMapper;
using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventRoleQueryHandler : IQueryHandler<GetAllEventRolesQuery, IEnumerable<EventRoleDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly IMapper _mapper;
    
    public GetAllEventRoleQueryHandler(
        ICurrentUser currentUser,
        IEventReadModelRepository eventReadModelRepository,
        IMapper mapper)
    {
        _currentUser = currentUser;
        _eventReadModelRepository = eventReadModelRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<EventRoleDto>> Handle(GetAllEventRolesQuery request, CancellationToken cancellationToken)
    {
        var eventRoles = await _eventReadModelRepository.GetEventRolesAsync(request.EventId);
        if (eventRoles == null)
        {
            throw new EventNotFoundException(request.EventId);
        }

        var eventRoleDtos = _mapper.Map<List<EventRoleDto>>(eventRoles);
        if (_currentUser.IsAuthenticated)
        {
            eventRoleDtos.ForEach(x =>
            {
                x.IsRegistered = eventRoles.Any(y => y.RegisteredStudents.Any(z => z.IdentityId == _currentUser.Id && z.Status == EventRegisterStatus.Approved));
            });
        }

        return eventRoleDtos;
    }
}