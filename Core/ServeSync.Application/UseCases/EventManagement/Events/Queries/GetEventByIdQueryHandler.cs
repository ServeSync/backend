using AutoMapper;
using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, EventDetailDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    private readonly IReadModelRepository<EventReadModel, Guid> _eventReadModelRepository;
    
    public GetEventByIdQueryHandler(
        ICurrentUser currentUser, 
        IMapper mapper,
        IReadModelRepository<EventReadModel, Guid> eventReadModelRepository)
    {
        _currentUser = currentUser;
        _mapper = mapper;
        _eventReadModelRepository = eventReadModelRepository;
    }
    
    public async Task<EventDetailDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventReadModelRepository.GetAsync(request.EventId);
        if (@event == null)
        {
            throw new EventNotFoundException(request.EventId);
        }
        
        var eventDetailDto = _mapper.Map<EventDetailDto>(@event);
        var canViewQrCode = await _currentUser.IsAdminAsync() || await _currentUser.IsStudentAffairAsync();
        if (!canViewQrCode)
        {
            eventDetailDto.AttendanceInfos.ForEach(x =>
            {
                x.Code = null;
                x.QrCodeUrl = null;
            });
        }
        
        if (_currentUser.IsAuthenticated)
        {
            eventDetailDto.IsAttendance = @event.AttendanceInfos.Any(x => x.AttendanceStudents.Any(y => y.IdentityId == _currentUser.Id));
            eventDetailDto.IsRegistered = @event.Roles.Any(x => x.RegisteredStudents.Any(y => y.IdentityId == _currentUser.Id && y.Status == EventRegisterStatus.Approved));
            eventDetailDto.Roles.ForEach(x =>
            {
                x.IsRegistered = @event.Roles.Any(y => y.RegisteredStudents.Any(z => z.IdentityId == _currentUser.Id && z.Id == x.Id));
            });
        }

        eventDetailDto.Status = eventDetailDto.GetCurrentStatus(DateTime.Now);
        return eventDetailDto;
    }
}