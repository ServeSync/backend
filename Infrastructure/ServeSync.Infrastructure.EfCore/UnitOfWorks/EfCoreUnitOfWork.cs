using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ServeSync.Application.SeedWorks.Data;

namespace ServeSync.Infrastructure.EfCore.UnitOfWorks;

public class EfCoreUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private IDbContextTransaction _transaction;
    
    public EfCoreUnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> CommitAsync()
    {
        return _dbContext.SaveChangesAsync();
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
            catch
            {
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
}