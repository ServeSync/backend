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
    private IDbContextTransaction? _transaction;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EfCoreUnitOfWork> _logger;
    private readonly List<IDomainEvent> _domainEvents = new();
    
    public EfCoreUnitOfWork(
        AppDbContext dbContext, 
        ILogger<EfCoreUnitOfWork> logger,
        IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task CommitAsync()
    {
        var domainEvents = await _dbContext.SaveChangesAppAsync();

        if (_transaction != null)
        {
            _domainEvents.AddRange(domainEvents);
        }
        else
        {
            await DispatchPersistedDomainEventsAsync(domainEvents);    
        }
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

                if (_domainEvents.Any())
                {
                    await DispatchPersistedDomainEventsAsync(_domainEvents);
                    _domainEvents.Clear();
                }
                
                await _transaction.DisposeAsync();
            }
            catch(Exception e)
            {
                _domainEvents.Clear();
                _logger.LogError("Commit transaction failed: {Message}", e.Message);
                if (autoRollbackOnFail)
                {
                    await RollbackTransactionAsync();
                }
                else
                {
                    throw;
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

    public IUnitOfWork InitScope()
    {
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }
}