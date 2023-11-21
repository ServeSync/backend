using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class ExchangeTenantAccessTokenCommand : ICommand<AuthCredentialDto>
{
    public Guid TenantId { get; set; }
    
    public ExchangeTenantAccessTokenCommand(Guid tenantId)
    {
        TenantId = tenantId;
    }
}