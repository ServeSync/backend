using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
}