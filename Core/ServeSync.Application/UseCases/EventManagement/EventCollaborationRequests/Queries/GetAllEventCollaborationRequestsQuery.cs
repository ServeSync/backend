using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Queries;

public class GetAllEventCollaborationRequestsQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<EventCollaborationRequestDto>>
{
    public string? Search { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EventType? EventType { get; set; }
    
    public GetAllEventCollaborationRequestsQuery(
        DateTime? startDate,
        DateTime? endDate,
        EventType? eventType,
        string? search, 
        int page, 
        int size, 
        string? sorting) : base(page, size, sorting)
    {
        StartDate = startDate;
        EndDate = endDate;
        EventType = eventType;
        Search = search;
    }
}
