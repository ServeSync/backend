using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;
public class GetAllEventOrganizationQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<EventOrganizationDto>>
{
    public string? Search { get; set; }
    
    public GetAllEventOrganizationQuery(
        string? search,
        int page, 
        int size,
        string? sorting) : base(page, size, sorting)
    {
        Search = search;
    }
}