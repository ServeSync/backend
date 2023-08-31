namespace PBL6.Domain.SeedWorks.Models.Interfaces;

public interface IHasSoftDeleteEntity<out TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
{
    public bool IsDeleted { get; }
    public string? DeletedBy { get; }

    void Delete(string deletedBy);
}