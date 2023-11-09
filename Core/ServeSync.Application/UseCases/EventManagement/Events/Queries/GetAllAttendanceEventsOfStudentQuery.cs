using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllAttendanceEventsOfStudentQuery : PagingRequestDto, IQuery<PagedResultDto<StudentAttendanceEventDto>>
{
    public Guid StudentId { get; set; }

    public GetAllAttendanceEventsOfStudentQuery(Guid studentId, int page, int size) : base(page, size)
    {
        StudentId = studentId;
    }
}