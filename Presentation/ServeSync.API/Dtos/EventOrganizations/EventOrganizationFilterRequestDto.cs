using ServeSync.Application.Common.Dtos;

namespace ServeSync.API.Dtos.EventOrganizations;

public class EventOrganizationFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
}