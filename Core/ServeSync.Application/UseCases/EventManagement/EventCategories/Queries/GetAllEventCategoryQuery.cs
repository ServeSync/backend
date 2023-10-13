using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;
public class GetAllEventCategoryQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<EventCategoryDto>>
{
    public string? Search { get; set; }
    
    public GetAllEventCategoryQuery(
        string? search,
        int page, 
        int size,
        string? sorting) : base(page, size, sorting)
    {
        Search = search;
    }
}