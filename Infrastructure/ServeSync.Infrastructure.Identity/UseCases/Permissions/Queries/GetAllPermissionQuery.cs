using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

public class GetAllPermissionQuery : IQuery<IEnumerable<PermissionDto>>
{
    public string Name { get; set; }
    
    public GetAllPermissionQuery(string name)
    {
        Name = name;
    }
}