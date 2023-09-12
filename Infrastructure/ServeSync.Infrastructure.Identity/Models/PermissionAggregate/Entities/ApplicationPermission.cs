using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

public class ApplicationPermission : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
}