using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

public class ApplicationPermission : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public ApplicationPermission(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        Description = Guard.NotNullOrEmpty(description, nameof(Description));
    }

    public void SetName(string name)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
    }
    
    public void SetDescription(string description)
    {
        Description = Guard.NotNullOrEmpty(description, nameof(Description));
    }

    private ApplicationPermission()
    {
        
    }
}