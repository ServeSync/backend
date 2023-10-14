namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IHasSoftDelete
{
    public bool IsDeleted { get; }
    public string? DeletedBy { get; }

    void Delete(string deletedBy);
}