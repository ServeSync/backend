using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Models;

public abstract class FullAuditableEntity<TKey> : AuditableEntity<TKey>, IFullAuditableEntity<TKey> where TKey : IEquatable<TKey>
{
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
    
    public void Delete(string deletedBy)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
        LastModified = DateTime.Now;
    }
}

public abstract class FullAuditableEntity : FullAuditableEntity<Guid>, IFullAuditableEntity
{
    
}