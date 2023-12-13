using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllRegisteredEventsOfStudentQuery : PagingRequestDto, IQuery<PagedResultDto<StudentRegisteredEventDto>>
{
    public Guid StudentId { get; set; }

    public GetAllRegisteredEventsOfStudentQuery(Guid studentId, int page, int size) : base(page, size)
    {
        StudentId = studentId;
    }
}