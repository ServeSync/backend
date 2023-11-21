using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Models.TenantAggregate.Exceptions;

public class TenantNotFoundException : ResourceNotFoundException
{
    public TenantNotFoundException(Guid id) : base(nameof(Tenant), id, ErrorCodes.TenantNotFound)
    {
    }
}