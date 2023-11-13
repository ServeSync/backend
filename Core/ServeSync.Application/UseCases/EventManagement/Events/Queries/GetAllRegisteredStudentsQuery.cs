using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllRegisteredStudentsQuery : PagingRequestDto, IQuery<PagedResultDto<RegisteredStudentInEventDto>>
{
    public Guid EventId { get; set; }
    public EventRegisterStatus? Status { get; set; }
    
    public GetAllRegisteredStudentsQuery(
        Guid eventId,
        EventRegisterStatus? status,
        int page, 
        int size) : base(page, size)
    {
        Status = status;
        EventId = eventId;
    }
}