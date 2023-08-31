namespace PBL6.Domain.SeedWorks.Models.Interfaces;

public interface IAuditableEntity<out TKey> : IEntity<TKey>, IHasSoftDeleteEntity<TKey> where TKey : IEquatable<TKey>
{
    public DateTimeOffset Created { get; }

    public string? CreatedBy { get; }

    public DateTimeOffset LastModified { get; }

    public string? LastModifiedBy { get; }

    void Create(string createdBy);

    void Update(string lastModifiedBy);
}

public interface IAuditableEntity : IAuditableEntity<Guid>
{
}