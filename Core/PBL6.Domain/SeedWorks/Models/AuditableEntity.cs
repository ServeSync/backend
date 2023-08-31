using PBL6.Domain.SeedWorks.Models.Interfaces;

namespace PBL6.Domain.SeedWorks.Models;

public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditableEntity<TKey> where TKey : IEquatable<TKey>
{
    public DateTimeOffset Created { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset LastModified { get; private set; }
    public string? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
    
    public void Create(string createdBy)
    {
        Created = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }
    
    public void Update(string lastModifiedBy)
    {
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = lastModifiedBy;
    }
    
    public void Delete(string deletedBy)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
    }
}

public abstract class AuditableEntity : AuditableEntity<Guid>, IAuditableEntity
{
    
}