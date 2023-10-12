using ServeSync.Application.Common.Dtos;

namespace ServeSync.API.Dtos.EventOrganizations;

public class EventOrganizationContactFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
}