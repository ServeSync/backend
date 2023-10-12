using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;

public class GetAllEventOrganizationContactQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<EventOrganizationContactDto>>
{
    public Guid EventOrganizationId { get; set; }
    public string? Search { get; set; }
    
    public GetAllEventOrganizationContactQuery(
        Guid eventOrganizationId, 
        string? search, 
        int page, 
        int size, 
        string? sorting) : base(page, size, sorting)
    {
        EventOrganizationId = eventOrganizationId;
        Search = search;
    }
}