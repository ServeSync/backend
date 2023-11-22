using ServeSync.Application.Common.Dtos;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationContactFilterRequestDto : PagingAndSortingRequestDto
{
    public OrganizationStatus? Status { get; set; }
    public string? Search { get; set; }
}