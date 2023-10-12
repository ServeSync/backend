using ServeSync.Application.Common.Dtos;

namespace ServeSync.API.Dtos.EventOrganizations;

public class EventActivityByCategoryFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
}