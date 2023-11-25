namespace ServeSync.Domain.SeedWorks.Models.Interfaces;

public interface IHasTenant
{
    Guid? TenantId { get; set; }
}