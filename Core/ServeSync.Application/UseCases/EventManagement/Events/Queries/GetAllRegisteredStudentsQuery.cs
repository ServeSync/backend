using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllRegisteredStudentsQuery : PagingRequestDto, IQuery<PagedResultDto<RegisteredStudentInEventDto>>
{
    public Guid EventId { get; set; }
    
    public GetAllRegisteredStudentsQuery(
        Guid eventId,
        int page, 
        int size) : base(page, size)
    {
        EventId = eventId;
    }
}