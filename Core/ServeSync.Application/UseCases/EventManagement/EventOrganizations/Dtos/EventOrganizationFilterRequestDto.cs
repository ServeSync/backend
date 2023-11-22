using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Validations;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationFilterRequestDto : PagingAndSortingRequestDto
{
    public OrganizationStatus? Status { get; set; }
    
    public string? Search { get; set; }
    
    [SortConstraint(Fields = $"{nameof(EventOrganization.Name)}, {nameof(EventOrganization.Email)}, {nameof(EventOrganization.PhoneNumber)}")]
    public override string? Sorting { get; set; } = string.Empty;
}