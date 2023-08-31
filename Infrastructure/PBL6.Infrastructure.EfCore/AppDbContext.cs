using MediatR;
using Microsoft.EntityFrameworkCore;
using PBL6.Domain.SeedWorks.Models;

namespace PBL6.Infrastructure.EfCore;

public class AppDbContext : DbContext
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