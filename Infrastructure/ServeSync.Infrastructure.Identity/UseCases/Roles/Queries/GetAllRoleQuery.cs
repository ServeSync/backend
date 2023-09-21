using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Queries;

public class GetAllRoleQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<RoleDto>>
{
    public string Name { get; set; }

    public GetAllRoleQuery(int page, int size, string sorting, string name) : base(page, size, sorting)
    {
        Name = name;
    }
}