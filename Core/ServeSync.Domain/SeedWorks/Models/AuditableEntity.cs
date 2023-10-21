using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Models;

public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditableEntity<TKey> where TKey : IEquatable<TKey>
{
    public DateTime Created { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public DateTime? LastModified { get; protected set; }
    public string? LastModifiedBy { get; protected set; }
    
    public void Create(string createdBy)
    {
        Created = DateTime.Now;
        CreatedBy = createdBy;
    }
    
    public void Update(string lastModifiedBy)
    {
        LastModified = DateTime.Now;
        LastModifiedBy = lastModifiedBy;
    }
}

public abstract class AuditableEntity : AuditableEntity<Guid>, IAuditableEntity
{
    
}