using System.Linq.Expressions;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly ICurrentUser _currentUser;
    private readonly IMediator _mediator;
    private readonly ILogger<AppDbContext> _logger;
    
    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUser currentUser,
        IMediator mediator,
        ILogger<AppDbContext> logger) : base(options)
    {
        _currentUser = currentUser;
        _mediator = mediator;
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IHasSoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var deletedCheck =
                    Expression.Lambda(
                        Expression.Equal(Expression.Property(parameter, nameof(IHasSoftDelete.IsDeleted)), 
                            Expression.Constant(false)),
                        parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
            }
        }
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        ProcessAuditEntityState();
        
        await DispatchDomainEventsAsync();
        
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker.Entries<IDomainModel>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .Distinct()
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            _logger.LogInformation("Dispatching domain event: {EventName}", domainEvent.GetType().Name);
            await _mediator.Publish(domainEvent);   
        }
    }
    
    private void ProcessAuditEntityState()
    {
        foreach (var entry in ChangeTracker.Entries<IHasAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Create(_currentUser.Id);
                    break;

                case EntityState.Modified:
                    entry.Entity.Update(_currentUser.Id);
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries<IHasSoftDelete>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.Delete(_currentUser.Id);
                    break;    
            }
        }
    }
}