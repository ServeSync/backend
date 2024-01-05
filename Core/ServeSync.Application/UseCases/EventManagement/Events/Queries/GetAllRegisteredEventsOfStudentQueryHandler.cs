using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllRegisteredEventsOfStudentQueryHandler : IQueryHandler<GetAllRegisteredEventsOfStudentQuery, PagedResultDto<StudentRegisteredEventDto>>
{
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly IMapper _mapper;

    public GetAllRegisteredEventsOfStudentQueryHandler(IEventReadModelRepository eventReadModelRepository, IMapper mapper)
    {
        _eventReadModelRepository = eventReadModelRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<StudentRegisteredEventDto>> Handle(GetAllRegisteredEventsOfStudentQuery request, CancellationToken cancellationToken)
    {
        var (events, total) = await _eventReadModelRepository.GetRegisteredEventsOfStudentAsync(request.StudentId, request.Status, request.Page, request.Size);
        
        var attendanceEventDtos = _mapper.Map<List<StudentRegisteredEventDto>>(events);
        foreach (var attendanceEventDto in attendanceEventDtos)
        {
            var eventReadModel = events.First(x => x.Id == attendanceEventDto.Id);
            attendanceEventDto.RoleId = eventReadModel.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == request.StudentId)).Id;
            attendanceEventDto.Role = eventReadModel.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == request.StudentId)).Name;
            attendanceEventDto.Score = eventReadModel.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == request.StudentId)).Score;
        }
        
        return new PagedResultDto<StudentRegisteredEventDto>(
            total,
            request.Size,
            attendanceEventDtos);
    }
}