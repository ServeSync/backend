using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

public class GetAllUsersQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<UserBasicInfoDto>>
{
    public string? Search { get; set; }
    
    public GetAllUsersQuery(
        string? search, 
        int page, 
        int size, 
        string? sorting) : base(page, size, sorting)
    {
        Search = search;
    }
}