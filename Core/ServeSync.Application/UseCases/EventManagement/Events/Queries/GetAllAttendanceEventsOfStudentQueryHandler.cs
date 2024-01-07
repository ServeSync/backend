using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllAttendanceEventsOfStudentQueryHandler : IQueryHandler<GetAllAttendanceEventsOfStudentQuery, PagedResultDto<StudentAttendanceEventDto>>
{
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly IMapper _mapper;

    public GetAllAttendanceEventsOfStudentQueryHandler(IEventReadModelRepository eventReadModelRepository, IMapper mapper)
    {
        _eventReadModelRepository = eventReadModelRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<StudentAttendanceEventDto>> Handle(GetAllAttendanceEventsOfStudentQuery request, CancellationToken cancellationToken)
    {
        var (events, total) = await _eventReadModelRepository.GetAttendanceEventsOfStudentAsync(request.StudentId, request.Page, request.Size, request.IsPaging);
        
        var attendanceEventDtos = _mapper.Map<List<StudentAttendanceEventDto>>(events);
        foreach (var attendanceEventDto in attendanceEventDtos)
        {
            var eventReadModel = events.First(x => x.Id == attendanceEventDto.Id);
            attendanceEventDto.Role = eventReadModel.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == request.StudentId)).Name;
            attendanceEventDto.AttendanceAt = eventReadModel.AttendanceInfos.SelectMany(x => x.AttendanceStudents).First(x => x.StudentId == request.StudentId).AttendanceAt;
            attendanceEventDto.Score = eventReadModel.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == request.StudentId)).Score;
        }
        
        return new PagedResultDto<StudentAttendanceEventDto>(
            total,
            request.Size,
            attendanceEventDtos);
    }
}