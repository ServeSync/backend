using ServeSync.Application.Common.Dtos;

namespace ServeSync.API.Dtos.EventCategories;

public class EventActivityByCategoryFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
}