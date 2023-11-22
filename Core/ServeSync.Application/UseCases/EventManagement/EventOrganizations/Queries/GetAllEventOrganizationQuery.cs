using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;
public class GetAllEventOrganizationQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<EventOrganizationDto>>
{
    public OrganizationStatus? Status { get; set; }
    public string? Search { get; set; }
    
    public GetAllEventOrganizationQuery(
        OrganizationStatus? status,
        string? search,
        int page, 
        int size,
        string? sorting) : base(page, size, sorting)
    {
        Status = status;
        Search = search;
    }
}