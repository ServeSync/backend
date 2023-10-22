using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

public class EventActivityByCategoryFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
}