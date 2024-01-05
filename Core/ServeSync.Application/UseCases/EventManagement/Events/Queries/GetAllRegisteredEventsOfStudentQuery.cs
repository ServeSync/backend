using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllRegisteredEventsOfStudentQuery : PagingRequestDto, IQuery<PagedResultDto<StudentRegisteredEventDto>>
{
    public Guid StudentId { get; set; }
    public EventStatus? Status { get; set; }

    public GetAllRegisteredEventsOfStudentQuery(Guid studentId, int page, int size, EventStatus? status) : base(page, size)
    {
        StudentId = studentId;
        Status = status;
    }
}