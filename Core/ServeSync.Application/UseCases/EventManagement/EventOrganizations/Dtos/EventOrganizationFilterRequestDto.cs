using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Validations;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
    
    [SortConstraint(Fields = $"{nameof(EventOrganization.Name)}, {nameof(EventOrganization.Email)}, {nameof(EventOrganization.PhoneNumber)}")]
    public override string? Sorting { get; set; } = string.Empty;
}