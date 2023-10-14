namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IFullAuditableEntity<out TKey> : IAuditableEntity<TKey>, IHasSoftDelete
    where TKey : IEquatable<TKey>
{
    
}

public interface IFullAuditableEntity : IFullAuditableEntity<Guid>
{
    
}