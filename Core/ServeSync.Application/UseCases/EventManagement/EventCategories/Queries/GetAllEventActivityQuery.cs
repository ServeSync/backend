using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;

public class GetAllEventActivityQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<EventActivityDto>>
{
    public Guid EventCategoryId { get; set; }
    public string? Search { get; set; }
    
    public GetAllEventActivityQuery(
        Guid eventCategoryId, 
        string? search, 
        int page, 
        int size, 
        string? sorting) : base(page, size, sorting)
    {
        EventCategoryId = eventCategoryId;
        Search = search;
    }
}