using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationContactFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
}