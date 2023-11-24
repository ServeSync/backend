using System.Linq.Expressions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.SeedWorks.Models.Interfaces;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore;

public class AppDbContext : IdentityDbContext<
    ApplicationUser, 
    ApplicationRole, 
    string, 
    IdentityUserClaim<string>, 
    ApplicationUserInRole,
    IdentityUserLogin<string>, 
    IdentityRoleClaim<string>, 
    IdentityUserToken<string>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AppDbContext> _logger;
    
    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUser currentUser,
        IMediator mediator,
        IServiceProvider serviceProvider,
        ILogger<AppDbContext> logger) : base(options)
    {
        _currentUser = currentUser;
        _mediator = mediator;
        _serviceProvider = serviceProvider;
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
        
        var domainEvents = await DispatchDomainEventsAsync();
        
        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchPersistedDomainEventsAsync(domainEvents);

        return result;
    }

    private async Task<IList<IDomainEvent>> DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker.Entries<IDomainModel>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .Distinct()
            .ToList();
        
        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            _logger.LogInformation("Dispatching domain event: {EventName}", domainEvent.GetType().Name);
            await _mediator.Publish(domainEvent);   
        }
        
        return domainEvents;
    }
    
    private async Task DispatchPersistedDomainEventsAsync(IList<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            _logger.LogInformation("Dispatching persisted domain event: {EventName}", domainEvent.GetType().Name);
            
            var domainEventHandlerType = typeof(IPersistedDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var domainEventHandlers = _serviceProvider.GetServices(domainEventHandlerType);
            
            foreach (var domainEventHandler in domainEventHandlers)
            {
                var handleMethod = domainEventHandlerType.GetMethod(nameof(IPersistedDomainEventHandler<IDomainEvent>.Handle));
                await (Task) handleMethod!.Invoke(domainEventHandler, new object[] { domainEvent, default(CancellationToken) });
            }
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