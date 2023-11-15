using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Infrastructure.EfCore.UnitOfWorks;

public class EfCoreUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private IDbContextTransaction _transaction;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EfCoreUnitOfWork> _logger;
    
    public EfCoreUnitOfWork(
        AppDbContext dbContext, 
        ILogger<EfCoreUnitOfWork> logger,
        IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<int> CommitAsync()
    {
        var result = await _dbContext.SaveChangesAsync();

        await DispatchPersistedDomainEventsAsync();
        
        return result;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(bool autoRollbackOnFail)
    {
        if (_transaction != null)
        {
            try
            {
                await CommitAsync();
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
            catch(Exception e)
            {
                _logger.LogError("Commit transaction failed: {Message}", e.Message);
                if (autoRollbackOnFail)
                {
                    await RollbackTransactionAsync();
                }
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }

    public IUnitOfWork InitScope()
    {
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }
    
    
    private async Task DispatchPersistedDomainEventsAsync()
    {
        var domainEntities = _dbContext.ChangeTracker.Entries<IDomainModel>()
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
            var domainEventHandlerType = typeof(IPersistedDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var domainEventHandlers = _serviceProvider.GetServices(domainEventHandlerType);
            
            foreach (var domainEventHandler in domainEventHandlers)
            {
                var handleMethod = domainEventHandlerType.GetMethod(nameof(IPersistedDomainEventHandler<IDomainEvent>.Handle));
                await (Task) handleMethod!.Invoke(domainEventHandler, new object[] { domainEvent, default(CancellationToken) });
            }
        }
    }
}