namespace PBL6.Application.SeedWorks.Data;

public interface IUnitOfWork
{
    Task<int> CommitAsync();

    Task BeginTransactionAsync();

    Task CommitTransactionAsync(bool autoRollbackOnFail = false);
    
    Task RollbackTransactionAsync();
}