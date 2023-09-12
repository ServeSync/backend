using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly IMediator _mediator;
    
    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await DispatchDomainEventsAsync();
        
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var entityType = typeof(Entity<>);
        var domainEntities = ChangeTracker
            .Entries()
            .Where(x => x.Entity.GetType().IsGenericType && x.Entity.GetType().GetGenericTypeDefinition() == entityType)
            .Where(x => ((dynamic)x.Entity).DomainEvents != null && ((dynamic)x.Entity).DomainEvents.Any());

        var domainEvents = domainEntities
            .Select(x => ((dynamic)x.Entity).DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => ((dynamic)entity.Entity).ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish((dynamic)domainEvent);
    }
}