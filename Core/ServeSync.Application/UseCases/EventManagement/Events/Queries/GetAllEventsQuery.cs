using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventsQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<FlatEventDto>>
{
    public string? Search { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EventType? EventType { get; set; }
    public EventStatus? EventStatus { get; set; }
    
    public GetAllEventsQuery(
        DateTime? startDate,
        DateTime? endDate,
        EventType? eventType,
        EventStatus? eventStatus,
        string? search, 
        int page, 
        int size, 
        string? sorting) : base(page, size, sorting)
    {
        StartDate = startDate;
        EndDate = endDate;
        EventType = eventType;
        EventStatus = eventStatus;
        Search = search;
    }
}