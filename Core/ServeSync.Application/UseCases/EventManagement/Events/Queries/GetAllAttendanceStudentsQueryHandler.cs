using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllAttendanceStudentsQueryHandler : IQueryHandler<GetAllAttendanceStudentsQuery, PagedResultDto<AttendanceStudentInEventDto>>
{
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly IMapper _mapper;
    public GetAllAttendanceStudentsQueryHandler(
        IMapper mapper,
        IEventReadModelRepository eventReadModelRepository)
    {
        _mapper = mapper;
        _eventReadModelRepository = eventReadModelRepository;
    }
    
    public async Task<PagedResultDto<AttendanceStudentInEventDto>> Handle(GetAllAttendanceStudentsQuery request, CancellationToken cancellationToken)
    {
        var (attendanceStudents, total) = await _eventReadModelRepository.GetPagedAttendanceStudentsInEventAsync(request.EventId, request.Page, request.Size);
        if (attendanceStudents == null)
        {
            throw new EventNotFoundException(request.EventId);
        }

        return new PagedResultDto<AttendanceStudentInEventDto>(
            total!.Value,
            request.Size,
            _mapper.Map<List<AttendanceStudentInEventDto>>(attendanceStudents)
        );
    }
}