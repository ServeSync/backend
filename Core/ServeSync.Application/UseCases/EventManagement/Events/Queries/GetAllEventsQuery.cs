using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventsQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<FlatEventDto>>
{
    public bool? IsPaging { get; set; }
    public string? Search { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EventType? EventType { get; set; }
    public EventStatus? EventStatus { get; set; }
    public IEnumerable<EventDefaultFilter>? DefaultFilters { get; set; }
    
    public GetAllEventsQuery(
        bool? isPaging,
        DateTime? startDate,
        DateTime? endDate,
        EventType? eventType,
        EventStatus? eventStatus,
        IEnumerable<EventDefaultFilter>? defaultFilters,
        string? search, 
        int page, 
        int size, 
        string? sorting) : base(page, size, sorting)
    {
        IsPaging = isPaging;
        StartDate = startDate;
        EndDate = endDate;
        EventType = eventType;
        EventStatus = eventStatus;
        DefaultFilters = defaultFilters;
        Search = search;
    }
}