namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IHasAuditable
{
    public DateTime Created { get; }

    public string? CreatedBy { get; }

    public DateTime? LastModified { get; }

    public string? LastModifiedBy { get; }

    void Create(string createdBy);

    void Update(string lastModifiedBy);
}