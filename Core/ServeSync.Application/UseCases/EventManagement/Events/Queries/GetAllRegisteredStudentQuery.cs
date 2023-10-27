using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllRegisteredStudentQuery : PagingRequestDto, IQuery<PagedResultDto<RegisteredStudentInEventDto>>
{
    public Guid EventRoleId { get; set; }
    
    public GetAllRegisteredStudentQuery(
        Guid eventRoleId,
        int page, 
        int size) : base(page, size)
    {
        EventRoleId = eventRoleId;
    }
}